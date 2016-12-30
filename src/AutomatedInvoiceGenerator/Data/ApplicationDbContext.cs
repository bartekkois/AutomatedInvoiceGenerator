using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AutomatedInvoiceGenerator.Models;

namespace AutomatedInvoiceGenerator.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<ServiceItemsSet> ServiceItemsSets { get; set; }

        public DbSet<ServiceItem> ServiceItems { get; set; }
        public DbSet<OneTimeServiceItem> OneTimeServiceItems { get; set; }
        public DbSet<SubscriptionServiceItem> SubscriptionServiceItems { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoicesItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
