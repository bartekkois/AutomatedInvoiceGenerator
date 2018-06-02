using System;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.DTO
{
    public class InvoiceItemShortDto
    {
        public int Id { get; set; }

        [Required]
        public DateTime? InvoicePeriodStartTime { get; set; }

        [Required]
        public DateTime? InvoicePeriodEndTime { get; set; }
    }
}