using System;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.DTO
{
    public class GenerateInvoicesDto
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }
    }
}
