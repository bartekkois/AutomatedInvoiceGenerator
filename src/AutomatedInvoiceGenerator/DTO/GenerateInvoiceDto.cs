using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.DTO
{
    public class GenerateInvoiceDto
    {
        [Required]
        public IEnumerable<int> Customers { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }
    }
}
