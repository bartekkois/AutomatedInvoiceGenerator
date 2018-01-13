using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models
{
    public class OneTimeServiceItem : ServiceItem
    {
        [Display(Name = "Data wykonania usługi:")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime? InstallationDate { get; set; }

        [Display(Name = "Zafakturowano:")]
        [Required]
        public bool IsInvoiced { get; set; }


        // Relationships
        public int? ServiceItemsSetId { get; set; }
        [Display(Name = "Zestaw usług:")]
        public virtual ServiceItemsSet ServiceItemsSet { get; set; }

        [Display(Name = "Pozycja faktury:")]
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
    }
}
