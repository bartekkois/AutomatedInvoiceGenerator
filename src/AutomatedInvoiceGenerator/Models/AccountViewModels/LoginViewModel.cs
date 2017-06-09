using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email:")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Hasło:")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}
