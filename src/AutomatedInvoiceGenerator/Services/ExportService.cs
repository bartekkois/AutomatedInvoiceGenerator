using AutomatedInvoiceGenerator.Data;
using AutomatedInvoiceGenerator.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;

namespace AutomatedInvoiceGenerator.Services
{
    public class ExportService : IExportService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ExportService> _logger;

        public ExportService(ApplicationDbContext context, ILogger<ExportService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void FlushOrCreateDirectory(string path)
        {
            try
            {
                if (Directory.Exists(path))
                    new DirectoryInfo(path).Delete(true);

                Directory.CreateDirectory(path);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void ExportInvoicesToComarchOptimaXMLFormat(DateTime exportStartDate, DateTime exportEndDate, string tempXMLDirectory)
        {
            try
            {
                foreach (var invoice in _context.Invoices.Where(i => i.InvoiceDate >= exportStartDate && i.InvoiceDate <= exportEndDate).Include(t => t.InvoiceItems).Include(c => c.Customer))
                {
                    using (FileStream fileStream = new FileStream(tempXMLDirectory + invoice.Customer.CustomerCode + "-FID" + invoice.Id + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xml", FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        _logger.LogInformation("eksport faktury o numerze wewnętrznym FID" + invoice.Id + " kontrahenta " + invoice.Customer.CustomerCode + " " + invoice.Customer.Name + " z dnia " + invoice.InvoiceDate);
                        XDocument invoiceXML = GenerateInvoiceXml(invoice);
                        invoiceXML.Save(fileStream);
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private static XDocument GenerateInvoiceXml(Invoice invoice)
        {
            string paymentMethod = "";

            switch (invoice.Customer.PaymentMethod)
            {
                case PaymentMethodType.BankTransfer:
                    paymentMethod = "przelew";
                    break;
                case PaymentMethodType.Cash:
                    paymentMethod = "gotowka";
                    break;
                case PaymentMethodType.Other:
                    paymentMethod = "inny";
                    break;
                default:
                    break;
            }

            try
            {
                return new XDocument(

                    new XDeclaration("1.0", "utf-8", "yes"),

                    new XElement("ROOT",
                        new XElement("DOKUMENT",
                            new XElement("NAGLOWEK",
                                  new XElement("GENERATOR", "eTabelki"),
                                  new XElement("TYP_DOKUMENTU", "302"),
                                  new XElement("RODZAJ_DOKUMENTU", "302000"),
                                  new XElement("KOREKTA", "0"),
                                  new XElement("DETAL", "0"),
                                  new XElement("TYP_NETTO_BRUTTO", (int)invoice.PriceCalculation),                                  // invoice price calculation : 1 - netto, 2 - gross
                                  new XElement("OPIS", invoice.Description),

                                  new XElement("PLATNIK",
                                                           new XElement("KOD", invoice.Customer.CustomerCode)
                                                           ),
                                  new XElement("ODBIORCA",
                                                           new XElement("KOD", invoice.Customer.ShippingCustomerCode)
                                                           ),

                                  new XElement("PLATNOSC",
                                                           new XElement("FORMA", paymentMethod)
                                                           // new XElement("TERMIN", "2016-04-19")                                 // (optional) Optima adds Payment Period by itself
                                                           )
                                  ),

                            new XElement("POZYCJE",

                               invoice.InvoiceItems.Select((invoiceItem, index) =>

                                    new XElement("POZYCJA",
                                                               new XElement("LP", index + 1),
                                                               new XElement("TOWAR",
                                                                                    new XElement("KOD", invoiceItem.RemoteSystemServiceCode),

                                                                                    new XElement("NAZWA", invoiceItem.Description)
                                                               ),

                                                               new XElement("STAWKA_VAT",
                                                                                    new XElement("STAWKA", invoiceItem.VATRate),
                                                                                    new XElement("FLAGA", "2"),
                                                                                    new XElement("ZRODLOWA", 0.00)
                                                               ),

                                                               new XElement("WARTOSC_NETTO_WAL", invoiceItem.NetValueAdded),       // Set netto/brutto depending on TYP_NETTO_BRUTTO
                                                               new XElement("WARTOSC_BRUTTO_WAL", invoiceItem.GrossValueAdded),
                                                               new XElement("ILOSC", invoiceItem.Quantity),
                                                               new XElement("JM", invoiceItem.Units)
                                      ))
                          ))));
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void CreateZipArchive(string sourceDirectory, string destinationFile)
        {
            try
            {
                ZipFile.CreateFromDirectory(sourceDirectory, destinationFile);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
