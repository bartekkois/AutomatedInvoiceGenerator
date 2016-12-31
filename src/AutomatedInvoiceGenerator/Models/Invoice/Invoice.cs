using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models
{
    public class Invoice : IAuditable
    {
        public int Id { get; set; }

        [Display(Name = "Opis:")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Data wystawienia:")]
        [Required]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Wyeksportowana:")]
        [Required]
        public bool IsExported { get; set; }

        [Display(Name = "Faktura liczona od:")]
        [Required]
        public PriceCalculationType PriceCalculation { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        // Relationships
        public int CustomerId { get; set; }
        [Display(Name = "Kontrahent:")]
        public virtual Customer Customer { get; set; }

        public int? ServiceItemSetId { get; set; }
        [Display(Name = "Zestaw usług:")]
        public virtual ServiceItemsSet ServiceItemsSet { get; set; }

        [Display(Name = "Elementy faktury:")]
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }

}
