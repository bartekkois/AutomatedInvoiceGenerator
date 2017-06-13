using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models
{
    public class Customer : IAuditable
    {
        public int Id { get; set; }

        [Display(Name = "Kod nabywcy:")]
        [Required(ErrorMessage = "Kod kontrahenta jest wymagany i musi być unikalny")]
        public string CustomerCode { get; set; }

        [Display(Name = "Kod odbiorcy:")]
        public string ShippingCustomerCode { get; set; }

        [Display(Name = "Nazwa:")]
        [Required(ErrorMessage = "Nazwa kontrahenta jest wymagana")]
        public string Name { get; set; }

        [Display(Name = "Marka:")]
        public string BrandName { get; set; }

        [Display(Name = "Lokalizacja usług:")]
        public string Location { get; set; }

        [Display(Name = "Notatki:")]
        public string Notes { get; set; }

        [Display(Name = "Indywidualny tag faktury kontrahenta:")]
        public string InvoiceCustomerSpecificTag { get; set; }

        [Display(Name = "Wysyłka faktur:")]
        [EnumDataType(typeof(InvoiceDeliveryType))]
        [Required]
        public InvoiceDeliveryType InvoiceDelivery { get; set; }

        [Display(Name = "Faktura liczona od:")]
        [EnumDataType(typeof(PriceCalculationType))]
        [Required]
        public PriceCalculationType PriceCalculation { get; set; }

        [Display(Name = "Typ płatności:")]
        [EnumDataType(typeof(PaymentMethodType))]
        [Required]
        public PaymentMethodType PaymentMethod { get; set; }

        [Display(Name = "Termin płatności (dni):")]
        public int PaymentPeriod { get; set; }

        [Display(Name = "Kontrahent VAT-EU:")]
        [Required]
        public bool IsVatEu { get; set; }

        [Display(Name = "Zablokowany abonent:")]
        [Required]
        public bool IsBlocked { get; set; }

        [Display(Name = "Zawieszone fakturowanie:")]
        [Required]
        public bool IsSuspended { get; set; }

        [Display(Name = "Zarchiwizowany:")]
        [Required]
        public bool IsArchived { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        // Relationships
        public int GroupId { get; set; }
        [Display(Name = "Grupa:")]
        public virtual Group Group { get; set; }

        [Display(Name = "Zestawy usług:")]
        public virtual ICollection<ServiceItemsSet> ServiceItemsSets { get; set; }

        [Display(Name = "Faktury:")]
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
