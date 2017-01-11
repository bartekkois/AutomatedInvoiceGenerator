using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutomatedInvoiceGenerator.Models.ManageViewModels
{
    public class ChangeUserRolesViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        public List<SelectListItem> Roles { get; set; }

        [Required]
        [Display(Name = "Uprawnienia")]
        public IEnumerable<string> UserRoles { get; set; }
    }
}
