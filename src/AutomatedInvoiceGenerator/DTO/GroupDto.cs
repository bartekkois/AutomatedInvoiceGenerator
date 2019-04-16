using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.DTO
{
    public class GroupDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa grupy jest wymagana")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Opis grupy jest wymagany")]
        public string Description { get; set; }

        public string Colour { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
