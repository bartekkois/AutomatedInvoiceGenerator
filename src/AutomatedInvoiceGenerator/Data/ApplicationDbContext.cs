using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AutomatedInvoiceGenerator.Models;
using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;

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
            foreach (var entityType in builder.Model.GetEntityTypes().Where(e => typeof(IAuditable).IsAssignableFrom(e.ClrType)))
            {
                builder.Entity(entityType.ClrType)
                    .Property<DateTime>("CreatedAt");

                builder.Entity(entityType.ClrType)
                    .Property<DateTime>("UpdatedAt");

                builder.Entity(entityType.ClrType)
                    .Property<string>("CreatedById");

                builder.Entity(entityType.ClrType)
                    .Property<string>("UpdatedById");
            }

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            AuditEntities();
            return base.SaveChanges();
        }

        private void AuditEntities()
        {
            string currentUserId = null;
            var currentUser = ClaimsPrincipal.Current;

            if (currentUser != null)
            {
                var identity = currentUser.Identity;
                if (identity != null)
                {
                    currentUserId = (from user in Users.Where(u => u.UserName == identity.Name) select user.Id).SingleOrDefault();
                }
            }

            foreach (EntityEntry<IAuditable> entry in ChangeTracker.Entries<IAuditable>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedById").CurrentValue = currentUserId;
                    entry.Property("CreatedAt").CurrentValue = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdatedById").CurrentValue = currentUserId;
                    entry.Property("UpdatedAt").CurrentValue = DateTime.Now;
                }
            }
        }
    }
}
