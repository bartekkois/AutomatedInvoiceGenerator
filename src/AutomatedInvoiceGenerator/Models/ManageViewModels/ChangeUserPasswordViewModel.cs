using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models.ManageViewModels
{
    public class ChangeUserPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "{0} musi mieć od {2} do {1} znaków.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("NewPassword", ErrorMessage = "Wprowadzone hasła różnią się od siebie.")]
        public string ConfirmPassword { get; set; }
    }
}
