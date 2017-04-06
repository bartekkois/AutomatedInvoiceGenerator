using AutomatedInvoiceGenerator.Models;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.DTO
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kod kontrahenta jest wymagany i musi być unikalny")]
        public string CustomerCode { get; set; }

        [Required(ErrorMessage = "Nazwa kontrahenta jest wymagana")]
        public string Name { get; set; }

        public string BrandName { get; set; }

        public string Location { get; set; }

        public string Notes { get; set; }

        public string InvoiceCustomerSpecificTag { get; set; }

        [EnumDataType(typeof(InvoiceDeliveryType))]
        [Required]
        public InvoiceDeliveryType InvoiceDelivery { get; set; }

        [EnumDataType(typeof(PriceCalculationType))]
        [Required]
        public PriceCalculationType PriceCalculation { get; set; }

        [EnumDataType(typeof(PaymentMethodType))]
        [Required]
        public PaymentMethodType PaymentMethod { get; set; }

        public int PaymentPeriod { get; set; }

        [Required]
        public bool IsVatEu { get; set; }

        [Required]
        public bool IsBlocked { get; set; }

        [Required]
        public bool IsSuspended { get; set; }

        [Required]
        public bool IsArchived { get; set; }

        [Required]
        public int GroupId { get; set; }
    }
}