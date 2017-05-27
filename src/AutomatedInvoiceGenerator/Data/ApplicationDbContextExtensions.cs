using AutomatedInvoiceGenerator.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutomatedInvoiceGenerator.Models.SampleData
{
    public static class ApplicationDbContextExtensions
    {
        public static async Task EnsureSeedData(this ApplicationDbContext context)
        {
            // Seed database - gropus
            if (!await context.Groups.AnyAsync())
            {
                await context.Groups.AddRangeAsync(
                    new Group
                    {
                        Name = "MPJ",
                        Description = "MetroPORT Jasło",
                    },
                    new Group
                    {
                        Name = "MPR",
                        Description = "MetroPORT Rzeszów",
                    },
                    new Group
                    {
                        Name = "GPJ",
                        Description = "GPON Jasło",
                    },
                    new Group
                    {
                        Name = "CCR",
                        Description = "GRZ",
                    },
                    new Group
                    {
                        Name = "WRZ",
                        Description = "War",
                    });

                await context.SaveChangesAsync();
            }

            // Seed database - customers
            if (!await context.Customers.AnyAsync())
            {
                await context.Customers.AddRangeAsync(
                    new Customer
                    {
                        CustomerCode = "CCR001",
                        GroupId = context.Groups.Where(g => g.Name == "CCR").ToList().First().Id,
                        Name = "LDD S. A.",
                        BrandName = "Say Say",
                        Location = "al. Piłsudskiego 190, 35-001 Rzeszów",
                        InvoiceCustomerSpecificTag = "Salon M154",
                        InvoiceDelivery = InvoiceDeliveryType.Direct,
                        PriceCalculation = PriceCalculationType.Netto,
                        PaymentMethod = PaymentMethodType.BankTransfer,
                        PaymentPeriod = 14,
                        IsVatEu = true,
                        IsBlocked = false,
                        IsSuspended = false,
                        IsArchived = false,
                    },
                    new Customer
                    {
                        CustomerCode = "CCR106",
                        GroupId = context.Groups.Where(g => g.Name == "CCR").ToList().First().Id,
                        Name = "Kadidi Sp. z o.o.",
                        BrandName = "Kadidi",
                        Location = "al. Piłsudskiego 190, 35-001 Rzeszów",
                        InvoiceDelivery = InvoiceDeliveryType.Direct,
                        PriceCalculation = PriceCalculationType.Netto,
                        PaymentMethod = PaymentMethodType.BankTransfer,
                        PaymentPeriod = 14,
                        IsVatEu = true,
                        IsBlocked = false,
                        IsSuspended = false,
                        IsArchived = false,
                    },
                    new Customer
                    {
                        CustomerCode = "MPJ156",
                        GroupId = context.Groups.Where(g => g.Name == "MPJ").ToList().First().Id,
                        Name = "Max-Vario Paweł Nowak",
                        BrandName = "Max-Vario",
                        Location = "ul. Kwiatowa 1120, 38-200 Jasło",
                        InvoiceDelivery = InvoiceDeliveryType.Email,
                        PriceCalculation = PriceCalculationType.Netto,
                        PaymentMethod = PaymentMethodType.BankTransfer,
                        PaymentPeriod = 14,
                        IsVatEu = false,
                        IsBlocked = false,
                        IsSuspended = false,
                        IsArchived = false,
                    },
                    new Customer
                    {
                        CustomerCode = "MPJ157",
                        GroupId = context.Groups.Where(g => g.Name == "MPJ").ToList().First().Id,
                        Name = "CDM Andrzej Jawo",
                        BrandName = "CDM",
                        Location = "ul. Towarowa 331, 38-200 Jasło",
                        InvoiceDelivery = InvoiceDeliveryType.Email,
                        PriceCalculation = PriceCalculationType.Netto,
                        PaymentMethod = PaymentMethodType.BankTransfer,
                        PaymentPeriod = 14,
                        IsVatEu = false,
                        IsBlocked = false,
                        IsSuspended = false,
                        IsArchived = true,
                    },
                    new Customer
                    {
                        CustomerCode = "MPR108",
                        GroupId = context.Groups.Where(g => g.Name == "MPR").ToList().First().Id,
                        Name = "DownRest Sp. z o.o.",
                        BrandName = "DownRest",
                        Location = "ul. Plac Wolności 333, 35-035 Rzeszów",
                        InvoiceDelivery = InvoiceDeliveryType.Email,
                        PriceCalculation = PriceCalculationType.Netto,
                        PaymentMethod = PaymentMethodType.BankTransfer,
                        PaymentPeriod = 14,
                        IsVatEu = false,
                        IsBlocked = false,
                        IsSuspended = false,
                        IsArchived = false,
                    },
                    new Customer
                    {
                        CustomerCode = "WRZ223",
                        GroupId = context.Groups.Where(g => g.Name == "WRZ").ToList().First().Id,
                        Name = "Aleksander Nowak",
                        Location = "War 800, 38-200 Jasło",
                        InvoiceDelivery = InvoiceDeliveryType.Email,
                        PriceCalculation = PriceCalculationType.Brutto,
                        PaymentMethod = PaymentMethodType.BankTransfer,
                        PaymentPeriod = 14,
                        IsVatEu = false,
                        IsBlocked = false,
                        IsSuspended = false,
                        IsArchived = false,
                    }
                    );

                await context.SaveChangesAsync();
            }

            // Seed database - service items sets
            if (!await context.ServiceItemsSets.AnyAsync())
            {
                await context.ServiceItemsSets.AddRangeAsync(
                    new ServiceItemsSet
                    {
                        CustomerId = context.Customers.Where(c => c.CustomerCode == "CCR001").ToList().First().Id,
                        Name = "Domyślny"
                    },
                    new ServiceItemsSet
                    {
                        CustomerId = context.Customers.Where(c => c.CustomerCode == "CCR106").ToList().First().Id,
                        Name = "Internet"
                    }
                    , new ServiceItemsSet
                    {
                        CustomerId = context.Customers.Where(c => c.CustomerCode == "CCR106").ToList().First().Id,
                        Name = "Telefon"
                    },
                    new ServiceItemsSet
                    {
                        CustomerId = context.Customers.Where(c => c.CustomerCode == "MPJ156").ToList().First().Id,
                        Name = "Domyślny"
                    },
                    new ServiceItemsSet
                    {
                        CustomerId = context.Customers.Where(c => c.CustomerCode == "MPJ157").ToList().First().Id,
                        Name = "Domyślny"
                    },
                    new ServiceItemsSet
                    {
                        CustomerId = context.Customers.Where(c => c.CustomerCode == "MPR108").ToList().First().Id,
                        Name = "Domyślny"
                    },
                    new ServiceItemsSet
                    {
                        CustomerId = context.Customers.Where(c => c.CustomerCode == "WRZ223").ToList().First().Id,
                        Name = "Domyślny"
                    }

                   );

                await context.SaveChangesAsync();
            }

            // Seed database - service items
            if (!await context.ServiceItems.AnyAsync())
            {
                await context.ServiceItems.AddRangeAsync(
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Internet,
                        RemoteSystemServiceCode = "1200",
                        Name = "Internet - abonament",
                        SubName = "fiberPORT 50/25 Mb/s",
                        IsSubNamePrinted = false,
                        SpecificLocation = "al. Piłsudskiego 190, 35-001 Rzeszów",
                        IsValueVariable = false,
                        NetValue = 120.00m,
                        Quantity = 1,
                        VATRate = 23,
                        GrossValueAdded = 147.60m,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = false,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "CCR001").ToList().First().ServiceItemsSets.First().Id,
                        StartDate = new DateTime(2012, 11, 10),
                    },
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Phone,
                        RemoteSystemServiceCode = "1210",
                        Name = "Telefon - abonament",
                        SubName = "179991085",
                        IsSubNamePrinted = false,
                        SpecificLocation = "al. Piłsudskiego 190, 35-001 Rzeszów",
                        IsValueVariable = false,
                        NetValue = 20.00m,
                        Quantity = 1,
                        VATRate = 23,
                        GrossValueAdded = 24.60m,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = false,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "CCR001").ToList().First().ServiceItemsSets.First().Id,
                        StartDate = new DateTime(2012, 11, 10),
                    },
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Phone,
                        RemoteSystemServiceCode = "1211",
                        Name = "Telefon - rozmowy telefoniczne",
                        SubName = "179991085",
                        IsSubNamePrinted = false,
                        SpecificLocation = "al. Piłsudskiego 190, 35-001 Rzeszów",
                        IsValueVariable = true,
                        VATRate = 23,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = false,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "CCR001").ToList().First().ServiceItemsSets.First().Id,
                        StartDate = new DateTime(2013, 05, 11),
                    },
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Internet,
                        RemoteSystemServiceCode = "1200",
                        Name = "Internet - abonament",
                        SubName = "fiberPORT 50/25 Mb/s",
                        IsSubNamePrinted = false,
                        SpecificLocation = "al. Piłsudskiego 190, 35-001 Rzeszów",
                        IsValueVariable = false,
                        NetValue = 119.00m,
                        Quantity = 1,
                        VATRate = 23,
                        GrossValueAdded = 146.36m,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = false,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "CCR106").ToList().First().ServiceItemsSets.First().Id,
                        StartDate = new DateTime(2013, 05, 11),
                    },
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Phone,
                        RemoteSystemServiceCode = "1210",
                        Name = "Telefon - abonament",
                        SubName = "179991129",
                        IsSubNamePrinted = false,
                        SpecificLocation = "al. Piłsudskiego 190, 35-001 Rzeszów",
                        IsValueVariable = false,
                        NetValue = 19.00m,
                        Quantity = 1,
                        VATRate = 23,
                        GrossValueAdded = 23.37m,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = false,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "CCR106").ToList().First().ServiceItemsSets.Skip(1).First().Id,
                        StartDate = new DateTime(2013, 05, 11),
                    },
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Phone,
                        RemoteSystemServiceCode = "1211",
                        Name = "Telefon - rozmowy telefoniczne",
                        SubName = "179991129",
                        IsSubNamePrinted = false,
                        SpecificLocation = "al. Piłsudskiego 190, 35-001 Rzeszów",
                        IsValueVariable = true,
                        VATRate = 23,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = false,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "CCR106").ToList().First().ServiceItemsSets.Skip(1).First().Id,
                        StartDate = new DateTime(2013, 05, 11),
                    },
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Internet,
                        RemoteSystemServiceCode = "1200",
                        Name = "Internet - abonament",
                        SubName = "fiberPORT 50/25 Mb/s",
                        IsSubNamePrinted = false,
                        SpecificLocation = "ul. Kwiatowa 1120, 38-200 Jasło",
                        IsValueVariable = false,
                        NetValue = 100.00m,
                        Quantity = 1,
                        VATRate = 23,
                        GrossValueAdded = 123.00m,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = false,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "MPJ156").ToList().First().ServiceItemsSets.First().Id,
                        StartDate = new DateTime(2011, 05, 11),
                    },
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Internet,
                        RemoteSystemServiceCode = "1200",
                        Name = "Internet - abonament",
                        SubName = "fiberPORT 50/25 Mb/s",
                        IsSubNamePrinted = false,
                        SpecificLocation = "ul. Towarowa 331, 38-200 Jasło",
                        IsValueVariable = false,
                        NetValue = 120.00m,
                        Quantity = 1,
                        VATRate = 23,
                        GrossValueAdded = 146.36m,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = true,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "MPJ157").ToList().First().ServiceItemsSets.First().Id,
                        StartDate = new DateTime(2012, 04, 22),
                    },
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Internet,
                        RemoteSystemServiceCode = "1200",
                        Name = "Internet - abonament",
                        SubName = "fiberPORT 100/100 Mb/s",
                        IsSubNamePrinted = false,
                        SpecificLocation = "ul. Plac Wolności 333, 35-035 Rzeszów",
                        IsValueVariable = false,
                        NetValue = 90.00m,
                        Quantity = 1,
                        VATRate = 23,
                        GrossValueAdded = 110.70m,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = false,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "MPR108").ToList().First().ServiceItemsSets.First().Id,
                        StartDate = new DateTime(2014, 03, 12),
                    },
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Phone,
                        RemoteSystemServiceCode = "1210",
                        Name = "Telefon - abonament",
                        SubName = "179991421",
                        IsSubNamePrinted = false,
                        SpecificLocation = "ul. Plac Wolności 333, 35-035 Rzeszów",
                        IsValueVariable = false,
                        NetValue = 19.00m,
                        Quantity = 1,
                        VATRate = 23,
                        GrossValueAdded = 23.37m,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = false,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "MPR108").ToList().First().ServiceItemsSets.First().Id,
                        StartDate = new DateTime(2014, 03, 12),
                    },
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Phone,
                        RemoteSystemServiceCode = "1211",
                        Name = "Telefon - rozmowy telefoniczne",
                        SubName = "179991421",
                        IsSubNamePrinted = false,
                        SpecificLocation = "ul. Plac Wolności 333, 35-035 Rzeszów",
                        IsValueVariable = true,
                        VATRate = 23,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = false,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "MPR108").ToList().First().ServiceItemsSets.First().Id,
                        StartDate = new DateTime(2014, 03, 12),
                    },
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Phone,
                        RemoteSystemServiceCode = "1275",
                        Name = "Opłata za dodatkowy publiczny adres IP",
                        IsSubNamePrinted = false,
                        SpecificLocation = "ul. Plac Wolności 333, 35-035 Rzeszów",
                        IsValueVariable = false,
                        NetValue = 10.00m,
                        Quantity = 1,
                        VATRate = 23,
                        GrossValueAdded = 12.30m,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = false,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "MPR108").ToList().First().ServiceItemsSets.First().Id,
                        StartDate = new DateTime(2014, 03, 12),
                    },
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Internet,
                        RemoteSystemServiceCode = "1200",
                        Name = "Internet - abonament",
                        SubName = "fiberPORT 150/5 Mb/s",
                        IsSubNamePrinted = true,
                        SpecificLocation = "War 800, 38-200 Jasło",
                        IsValueVariable = false,
                        NetValue = 46.34m,
                        Quantity = 1,
                        VATRate = 23,
                        GrossValueAdded = 57.00m,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = false,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "WRZ223").ToList().First().ServiceItemsSets.First().Id,
                        StartDate = new DateTime(2015, 12, 12),
                    },
                    new SubscriptionServiceItem
                    {
                        ServiceCategoryType = ServiceCategoryType.Phone,
                        RemoteSystemServiceCode = "1208",
                        Name = "Dzierżawa modemu z funckją routera Wi-Fi",
                        IsSubNamePrinted = false,
                        SpecificLocation = "War 800, 38-200 Jasło",
                        IsValueVariable = false,
                        NetValue = 4.89m,
                        Quantity = 1,
                        VATRate = 23,
                        GrossValueAdded = 6.00m,
                        IsManual = false,
                        IsSuspended = false,
                        IsBlocked = false,
                        IsArchived = false,
                        ServiceItemsSetId = context.Customers.Include(serviceItemsSet => serviceItemsSet.ServiceItemsSets).Where(customer => customer.CustomerCode == "WRZ223").ToList().First().ServiceItemsSets.First().Id,
                        StartDate = new DateTime(2015, 12, 12),
                    }
                   );

                await context.SaveChangesAsync();
            }
        }

        public static async Task EnsureSeedRoles(this RoleManager<IdentityRole> roleManager)
        {
            // Seed database - roles
            var roles = new List<string> { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public static async Task EnsureSeedAdministrators(this UserManager<ApplicationUser> userManager)
        {
            // Seed database - administrators
            var admin = new ApplicationUser
            {
                UserName = "admin@company.com",
                Email = "admin@company.com",
            };

            if (await userManager.FindByNameAsync(admin.UserName) == null)
            {
                await userManager.CreateAsync(admin, "admin");
            }

            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            if (!await userManager.IsInRoleAsync(admin, "User"))
            {
                await userManager.AddToRoleAsync(admin, "User");
            }
        }

        public static async Task EnsureSeedUsers(this UserManager<ApplicationUser> userManager)
        {
            // Seed database - users
            var user = new ApplicationUser
            {
                UserName = "user@company.com",
                Email = "user@company.com",
            };

            if (await userManager.FindByNameAsync(user.UserName) == null)
            {
                await userManager.CreateAsync(user, "user");
            }

            if (!await userManager.IsInRoleAsync(user, "User"))
            {
                await userManager.AddToRoleAsync(user, "User");
            }
        }
    }
}
