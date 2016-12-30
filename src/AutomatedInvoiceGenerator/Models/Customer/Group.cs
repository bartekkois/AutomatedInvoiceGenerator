using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa:")]
        [Required(ErrorMessage = "Nazwa grupy jest wymagana")]
        public string Name { get; set; }

        [Display(Name = "Opis:")]
        [Required(ErrorMessage = "Opis grupy jest wymagany")]
        public string Description { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        // Relationships
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
