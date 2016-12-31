using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models
{
    public class ServiceItemsSet : IAuditable
    {
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        // Relationships
        public int CustomerId { get; set; }
        [Display(Name = "Kontrahent:")]
        public virtual Customer Customer { get; set; }

        [Display(Name = "Faktury:")]
        public virtual ICollection<Invoice> Invoices { get; set; }

        [Display(Name = "Usługi jednorazowe:")]
        public virtual ICollection<OneTimeServiceItem> OneTimeServiceItems { get; set; }

        [Display(Name = "Usługi abonamentowe:")]
        public virtual ICollection<SubscriptionServiceItem> SubscriptionServiceItems { get; set; }
    }
}
