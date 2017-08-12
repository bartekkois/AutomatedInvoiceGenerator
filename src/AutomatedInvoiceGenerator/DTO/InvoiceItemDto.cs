using System;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.DTO
{
    public class InvoiceItemDto
    {
        public int Id { get; set; }

        [Required]
        public string RemoteSystemServiceCode { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public string Units { get; set; }

        [Required]
        public decimal NetUnitPrice { get; set; }

        [Required]
        public decimal NetValueAdded { get; set; }

        [Required]
        public decimal VATRate { get; set; }

        [Required]
        public decimal GrossValueAdded { get; set; }

        [Required]
        public DateTime? InvoicePeriodStartTime { get; set; }

        [Required]
        public DateTime? InvoicePeriodEndTime { get; set; }


        // Relationships
        public int InvoiceId { get; set; }

        public int? ServiceItemId { get; set; }
    }
}
