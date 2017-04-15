using AutomatedInvoiceGenerator.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.DTO
{
    public class OneTimeServiceItemDto
    {
        public int Id { get; set; }

        public ServiceCategoryType ServiceCategoryType { get; set; }

        public string RemoteSystemServiceCode { get; set; }

        [Required(ErrorMessage = "Nazwa usługi jest wymagana")]
        public string Name { get; set; }

        [Display(Name = "Szczegóły usługi:")]
        public string SubName { get; set; }

        [Required]
        public bool IsSubNamePrinted { get; set; }

        [Required(ErrorMessage = "Lokalizacja usługi jest wymagana")]
        public string SpecificLocation { get; set; }

        public string ServiceItemCustomerSpecificTag { get; set; }

        public string Notes { get; set; }

        [Required]
        public bool IsValueVariable { get; set; }

        [Required]
        public decimal NetValue { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal VATRate { get; set; }

        [Required]
        public decimal GrossValueAdded { get; set; }

        [Required]
        public bool IsManual { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        [Required]
        public bool IsSuspended { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime? InstallationDate { get; set; }

        [Required]
        public bool IsInvoiced { get; set; }

        [Required]
        public bool IsArchived { get; set; }
    }
}
