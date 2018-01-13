using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AutomatedInvoiceGenerator.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<IdentityUserRole<string>> Roles { get; } = new List<IdentityUserRole<string>>();
    }
}