using AutomatedInvoiceGenerator.Data;
using AutomatedInvoiceGenerator.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using static AutomatedInvoiceGenerator.Helpers.DateTimeHelper;

namespace AutomatedInvoiceGenerator.Services
{
    public class GenerateInvoiceService : IGenerateInvoiceService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GenerateInvoiceService> _logger;

        public GenerateInvoiceService(ApplicationDbContext context, ILogger<GenerateInvoiceService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task GenerateInvoice(Customer customer, DateTime invoiceDate)
        {
            _logger.LogInformation("== Generowanie faktury dla abonenta (" + customer.CustomerCode + ") " + customer.Name);

            // Customer
            if (_context.Invoices.Where(i => i.Customer == customer && i.InvoiceDate.Month == invoiceDate.Month).Any())
            {
                var message = "abonent (" + customer.CustomerCode + ") " + customer.Name + " posiada już fakturę wystawioną w podanym miesiącu";
                _logger.LogError(message);
                throw new Exception(message);
            }

            if (customer.IsSuspended == true)
            {
                var message = "abonent (" + customer.CustomerCode + ") " + customer.Name + " zawieszony";
                _logger.LogError(message);
                throw new Exception(message);
            }

            if (customer.IsArchived == true)
            {
                var message = "abonent (" + customer.CustomerCode + ") " + customer.Name + " zarchiwizowany";
                _logger.LogError(message);
                throw new Exception(message);
            }

            // Service Items Sets
            foreach (ServiceItemsSet serviceItemsSet in _context.ServiceItemsSets.Where(s => s.Customer == customer))
            {
                // Invoice
                Invoice invoice = new Invoice()
                {
                    Customer = customer,
                    Description = customer.InvoiceCustomerSpecificTag,
                    InvoiceDelivery = customer.InvoiceDelivery,
                    PriceCalculation = customer.PriceCalculation,
                    PaymentMethod = customer.PaymentMethod,
                    PaymentPeriod = customer.PaymentPeriod,
                    InvoiceDate = invoiceDate,
                    IsExported = false
                };

                await _context.Invoices.AddAsync(invoice);

                // One Time Service Items
                foreach (OneTimeServiceItem oneTimeServiceItem in _context.OneTimeServiceItems.Where(s => s.ServiceItemsSet == serviceItemsSet))
                {
                    // Check if the item is archived or suspended
                    if (oneTimeServiceItem.IsSuspended == true)
                        continue;

                    if (oneTimeServiceItem.IsArchived == true)
                        continue;

                    // Check if the item was previously invoiced
                    if (oneTimeServiceItem.IsInvoiced == true)
                        continue;


                    // Build Invoice Item
                    string currentItemDetails = "";
                    if (oneTimeServiceItem.IsSubNamePrinted == true)
                        currentItemDetails = oneTimeServiceItem.SubName;
                    else
                        currentItemDetails = "";

                    InvoiceItem invoiceItem = new InvoiceItem()
                    {
                        Invoice = invoice,
                        RemoteSystemServiceCode = oneTimeServiceItem.RemoteSystemServiceCode,
                        Description = oneTimeServiceItem.Name.Replace("%DETALE%", currentItemDetails),
                        Quantity = oneTimeServiceItem.Quantity,
                        Units = "usł.",                                                                 // TO BE FIXED !!!!!
                        NetUnitPrice = oneTimeServiceItem.NetValue,
                        NetValueAdded = oneTimeServiceItem.NetValue * oneTimeServiceItem.Quantity,
                        VATRate = oneTimeServiceItem.VATRate,
                        GrossValueAdded = oneTimeServiceItem.GrossValueAdded,
                        InvoicePeriodStartTime = oneTimeServiceItem.InstallationDate,
                        InvoicePeriodEndTime = oneTimeServiceItem.InstallationDate
                    };

                    await _context.InvoicesItems.AddAsync(invoiceItem);

                    // Update IsInvoiced property
                    oneTimeServiceItem.IsInvoiced = true;
                    _context.OneTimeServiceItems.Update(oneTimeServiceItem);

                    _logger.LogInformation("dodano usługę jednorazową: (" + invoiceItem.RemoteSystemServiceCode + ") " + invoiceItem.Description);
                }

                // Subscription Service Items
                foreach (SubscriptionServiceItem subscriptionServiceItem in _context.SubscriptionServiceItems.Where(s => s.ServiceItemsSet == serviceItemsSet))
                {
                    // Check if the item is archived or suspended
                    if (subscriptionServiceItem.IsSuspended == true)
                        continue;

                    if (subscriptionServiceItem.IsArchived == true)
                        continue;


                    // Calculate the period item must be invoiced for
                    DateTimePeriod invoicingPeriod = new DateTimePeriod();

                    var lastlyInvoicedItem = _context.InvoicesItems.Where(i => i.ServiceItem == subscriptionServiceItem).OrderByDescending(t => t.InvoicePeriodEndTime).FirstOrDefault();

                    if (lastlyInvoicedItem == null && subscriptionServiceItem.StartDate.HasValue)
                    {
                        invoicingPeriod.StartDate = subscriptionServiceItem.StartDate.Value;
                        invoicingPeriod.EndDate = CalculateLastSecondOfTheMonth(invoiceDate);
                    }
                    else
                    {
                        if (lastlyInvoicedItem.InvoicePeriodEndTime < invoiceDate && lastlyInvoicedItem.InvoicePeriodEndTime.HasValue)
                        {
                            invoicingPeriod.StartDate = lastlyInvoicedItem.InvoicePeriodEndTime.Value.AddDays(1);
                            invoicingPeriod.EndDate = CalculateLastSecondOfTheMonth(invoiceDate);
                        }
                    }

                    if (subscriptionServiceItem.EndDate.HasValue)
                        invoicingPeriod.EndDate = subscriptionServiceItem.EndDate;

                    if (!invoicingPeriod.IsValid())
                    {
                        var message = "wystąpił błąd podczas obliczania okresu fakturowania usługi abonamentowej: " + subscriptionServiceItem.Name + " " + subscriptionServiceItem.SubName + " kontrahenta: " + customer.CustomerCode + " " + customer.Name;
                        _logger.LogError(message);
                        throw new Exception(message);
                    }

                    // Build Invoice Items
                    foreach (DateTimePeriod invoicePreiod in MonthsPeriodsBetweenDates(invoicingPeriod))
                    {
                        string currentItemDetails = "";
                        if (subscriptionServiceItem.IsSubNamePrinted == true)
                            currentItemDetails = subscriptionServiceItem.SubName;
                        else
                            currentItemDetails = "";

                        decimal invoicePeriodAsFractionOfMonth = CalculatePeriodAsFractionOfMonth(invoicePreiod.StartDate.Value, invoicePreiod.EndDate.Value);

                        InvoiceItem invoiceItem = new InvoiceItem()
                        {
                            Invoice = invoice,
                            RemoteSystemServiceCode = subscriptionServiceItem.RemoteSystemServiceCode,
                            Description = subscriptionServiceItem.Name.Replace("%DETALE%", currentItemDetails).Replace("%OKRES%", invoicePreiod.StartDate.Value.ToString("MM/yyyy")),
                            Quantity = subscriptionServiceItem.Quantity,
                            Units = "usł.",                                                             // TO BE FIXED !!!!!
                            NetUnitPrice = Math.Round(subscriptionServiceItem.NetValue * invoicePeriodAsFractionOfMonth, 2),
                            NetValueAdded = Math.Round(subscriptionServiceItem.NetValue * invoicePeriodAsFractionOfMonth * subscriptionServiceItem.Quantity, 2),
                            VATRate = subscriptionServiceItem.VATRate,
                            GrossValueAdded = Math.Round(subscriptionServiceItem.GrossValueAdded * invoicePeriodAsFractionOfMonth, 2),
                            InvoicePeriodStartTime = invoicePreiod.StartDate,
                            InvoicePeriodEndTime = invoicePreiod.EndDate
                        };

                        await _context.InvoicesItems.AddAsync(invoiceItem);

                        _logger.LogInformation("dodano usługę abonamentową: (" + invoiceItem.RemoteSystemServiceCode + ") " + invoiceItem.Description);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
