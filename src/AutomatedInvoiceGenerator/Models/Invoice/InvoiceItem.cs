using System;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models
{
    public class InvoiceItem : IAuditable
    {
        public int Id { get; set; }

        [Display(Name = "Kod usługi (obcy):")]
        [Required]
        public string RemoteSystemServiceCode { get; set; }

        [Display(Name = "Nazwa:")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Ilość:")]
        [Required]
        public decimal Quantity { get; set; }

        [Display(Name = "Jednostka miary:")]
        [Required]
        public string Units { get; set; }

        [Display(Name = "Cena netto:")]
        [Required]
        public decimal NetUnitPrice { get; set; }

        [Display(Name = "Wartość netto:")]
        [Required]
        public decimal NetValueAdded { get; set; }

        [Display(Name = "Stawka VAT:")]
        [Required]
        public decimal VATRate { get; set; }

        [Display(Name = "Wartość brutto:")]
        [Required]
        public decimal GrossValueAdded { get; set; }

        [Display(Name = "Początek okresu fakturowania:")]
        [Required]
        public DateTime? InvoicePeriodStartTime { get; set; }

        [Display(Name = "Koniec okresu fakturowania:")]
        [Required]
        public DateTime? InvoicePeriodEndTime { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        // Relationships
        public int InvoiceId { get; set; }
        [Display(Name = "Faktura:")]
        public virtual Invoice Invoice { get; set; }

        public int? ServiceItemId { get; set; }
        [Display(Name = "Usługa:")]
        public virtual ServiceItem ServiceItem { get; set; }
    }

}
