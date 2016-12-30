using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using AutomatedInvoiceGenerator.Data;
using AutomatedInvoiceGenerator.Models;

namespace AutomatedInvoiceGenerator.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20161230154200_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BrandName");

                    b.Property<string>("CustomerCode")
                        .IsRequired();

                    b.Property<int>("GroupId");

                    b.Property<string>("InvoiceCustomerSpecificTag");

                    b.Property<int>("InvoiceDelivery");

                    b.Property<bool>("IsArchived");

                    b.Property<bool>("IsBlocked");

                    b.Property<bool>("IsSuspended");

                    b.Property<bool>("IsVatEu");

                    b.Property<string>("Location");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Notes");

                    b.Property<int>("PaymentMethod");

                    b.Property<int>("PaymentPeriod");

                    b.Property<int>("PriceCalculation");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<DateTime>("InvoiceDate");

                    b.Property<bool>("IsExported");

                    b.Property<int>("PriceCalculation");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int?>("ServiceItemSetId");

                    b.Property<int?>("ServiceItemsSetId");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ServiceItemsSetId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.InvoiceItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<decimal>("GrossValueAdded");

                    b.Property<int>("InvoiceId");

                    b.Property<DateTime>("InvoicePeriodEndTime");

                    b.Property<DateTime>("InvoicePeriodStartTime");

                    b.Property<decimal>("NetUnitPrice");

                    b.Property<decimal>("NetValueAdded");

                    b.Property<decimal>("Quantity");

                    b.Property<string>("RemoteSystemServiceCode")
                        .IsRequired();

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int?>("ServiceItemId");

                    b.Property<string>("Units")
                        .IsRequired();

                    b.Property<decimal>("VATRate");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.HasIndex("ServiceItemId");

                    b.ToTable("InvoicesItems");
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.ServiceItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<decimal>("GrossValueAdded");

                    b.Property<bool>("IsArchived");

                    b.Property<bool>("IsBlocked");

                    b.Property<bool>("IsManual");

                    b.Property<bool>("IsSubNamePrinted");

                    b.Property<bool>("IsSuspended");

                    b.Property<bool>("IsValueVariable");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("NetValue");

                    b.Property<string>("Notes");

                    b.Property<int>("Quantity");

                    b.Property<string>("RemoteSystemServiceCode")
                        .IsRequired();

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<int>("ServiceCategoryType");

                    b.Property<string>("ServiceItemCustomerSpecificTag");

                    b.Property<string>("SpecificLocation")
                        .IsRequired();

                    b.Property<string>("SubName");

                    b.Property<decimal>("VATRate");

                    b.HasKey("Id");

                    b.ToTable("ServiceItems");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ServiceItem");
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.ServiceItemsSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("ServiceItemsSets");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.OneTimeServiceItem", b =>
                {
                    b.HasBaseType("AutomatedInvoiceGenerator.Models.ServiceItem");

                    b.Property<DateTime?>("InstallationDate")
                        .IsRequired();

                    b.Property<int?>("InvoiceItemsId");

                    b.Property<bool>("IsInvoiced");

                    b.Property<int>("ServiceItemsSetId");

                    b.HasIndex("InvoiceItemsId");

                    b.HasIndex("ServiceItemsSetId");

                    b.ToTable("OneTimeServiceItem");

                    b.HasDiscriminator().HasValue("OneTimeServiceItem");
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.SubscriptionServiceItem", b =>
                {
                    b.HasBaseType("AutomatedInvoiceGenerator.Models.ServiceItem");

                    b.Property<DateTime?>("EndDate");

                    b.Property<int?>("InvoiceItemsId");

                    b.Property<int>("ServiceItemsSetId");

                    b.Property<DateTime?>("StartDate")
                        .IsRequired();

                    b.HasIndex("InvoiceItemsId");

                    b.HasIndex("ServiceItemsSetId");

                    b.ToTable("SubscriptionServiceItem");

                    b.HasDiscriminator().HasValue("SubscriptionServiceItem");
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.Customer", b =>
                {
                    b.HasOne("AutomatedInvoiceGenerator.Models.Group", "Group")
                        .WithMany("Customers")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.Invoice", b =>
                {
                    b.HasOne("AutomatedInvoiceGenerator.Models.Customer", "Customer")
                        .WithMany("Invoices")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AutomatedInvoiceGenerator.Models.ServiceItemsSet", "ServiceItemsSet")
                        .WithMany("Invoices")
                        .HasForeignKey("ServiceItemsSetId");
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.InvoiceItem", b =>
                {
                    b.HasOne("AutomatedInvoiceGenerator.Models.Invoice", "Invoice")
                        .WithMany("InvoiceItems")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AutomatedInvoiceGenerator.Models.ServiceItem", "ServiceItem")
                        .WithMany()
                        .HasForeignKey("ServiceItemId");
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.ServiceItemsSet", b =>
                {
                    b.HasOne("AutomatedInvoiceGenerator.Models.Customer", "Customer")
                        .WithMany("ServiceItemsSets")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AutomatedInvoiceGenerator.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AutomatedInvoiceGenerator.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AutomatedInvoiceGenerator.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.OneTimeServiceItem", b =>
                {
                    b.HasOne("AutomatedInvoiceGenerator.Models.InvoiceItem", "InvoiceItems")
                        .WithMany()
                        .HasForeignKey("InvoiceItemsId");

                    b.HasOne("AutomatedInvoiceGenerator.Models.ServiceItemsSet", "ServiceItemsSet")
                        .WithMany("OneTimeServiceItems")
                        .HasForeignKey("ServiceItemsSetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AutomatedInvoiceGenerator.Models.SubscriptionServiceItem", b =>
                {
                    b.HasOne("AutomatedInvoiceGenerator.Models.InvoiceItem", "InvoiceItems")
                        .WithMany()
                        .HasForeignKey("InvoiceItemsId");

                    b.HasOne("AutomatedInvoiceGenerator.Models.ServiceItemsSet", "ServiceItemsSet")
                        .WithMany("SubscriptionServiceItems")
                        .HasForeignKey("ServiceItemsSetId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
