using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models
{
    public abstract class ServiceItem : IAuditable
    {
        public int Id { get; set; }

        [Display(Name = "Rodziaj usługi: ")]
        [Required(ErrorMessage = "Rodzaj usługi jest wymagany")]
        public ServiceCategoryType ServiceCategoryType { get; set; }

        [Display(Name = "Kod usługi (obcy):")]
        [Required(ErrorMessage = "Kod usługi (obcy) jest wymagany")]
        public string RemoteSystemServiceCode { get; set; }

        [Display(Name = "Nazwa usługi:")]
        [Required(ErrorMessage = "Nazwa usługi jest wymagana")]
        public string Name { get; set; }

        [Display(Name = "Szczegóły usługi:")]
        public string SubName { get; set; }

        [Display(Name = "Szczegóły usługi są drukowane:")]
        [Required]
        public bool IsSubNamePrinted { get; set; }

        [Display(Name = "Lokalizacja:")]
        [Required(ErrorMessage = "Lokalizacja usługi jest wymagana")]
        public string SpecificLocation { get; set; }

        [Display(Name = "Tag usługi w systemie kontrahenta:")]
        public string ServiceItemCustomerSpecificTag { get; set; }

        [Display(Name = "Notatki:")]
        public string Notes { get; set; }

        [Display(Name = "Wartość zmienna:")]
        [Required]
        public bool IsValueVariable { get; set; }

        [Display(Name = "Wartość netto:")]
        [Required]
        public decimal NetValue { get; set; }

        [Display(Name = "Liczba/ilość:")]
        [Required]
        public int Quantity { get; set; }

        [Display(Name = "Stawka VAT:")]
        [Required]
        public decimal VATRate { get; set; }

        [Display(Name = "Wartość brutto:")]
        [Required]
        public decimal GrossValueAdded { get; set; }

        [Display(Name = "Wymaga uwagi:")]
        [Required]
        public bool IsManual { get; set; }

        [Display(Name = "Zablokowane usługi:")]
        [Required]
        public bool IsBlocked { get; set; }

        [Display(Name = "Zawieszone fakturowanie:")]
        [Required]
        public bool IsSuspended { get; set; }

        [Display(Name = "Zarchiwizowana:")]
        [Required]
        public bool IsArchived { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
