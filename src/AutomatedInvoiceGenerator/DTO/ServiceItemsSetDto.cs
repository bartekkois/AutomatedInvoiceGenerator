using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.DTO
{
    public class ServiceItemsSetDto
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public virtual ICollection<OneTimeServiceItemDto> OneTimeServiceItems { get; set; }

        public virtual ICollection<SubscriptionServiceItemDto> SubscriptionServiceItems { get; set; }
    }
}
