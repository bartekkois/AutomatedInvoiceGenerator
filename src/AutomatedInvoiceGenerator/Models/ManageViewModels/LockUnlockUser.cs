using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models.ManageViewModels
{
    public class LockUnlockUserViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Zablokowany")]
        public bool IsLocked { get; set; }
    }
}
