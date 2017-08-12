using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.DTO
{
    public class CustomerShortDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kod kontrahenta jest wymagany i musi być unikalny")]
        public string CustomerCode { get; set; }

        public string ShippingCustomerCode { get; set; }

        [Required(ErrorMessage = "Nazwa kontrahenta jest wymagana")]
        public string Name { get; set; }

        public string BrandName { get; set; }

        public string Location { get; set; }

        public string Notes { get; set; }
    }
}