using System;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models
{
    public class SubscriptionServiceItem : ServiceItem
    {
        [Display(Name = "Rozpoczęcie świadczenia usługi:")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Zakończenie świadczenia usługi:")]
        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }


        // Relationships
        public int ServiceItemsSetId { get; set; }
        [Display(Name = "Zestaw usług:")]
        public virtual ServiceItemsSet ServiceItemsSet { get; set; }

        [Display(Name = "Pozycja faktury:")]
        public virtual InvoiceItem InvoiceItems { get; set; }
    }
}
