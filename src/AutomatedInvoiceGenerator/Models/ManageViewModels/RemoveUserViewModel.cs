using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models.ManageViewModels
{
    public class RemoveUserViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}
