using System.Collections.Generic;

namespace AutomatedInvoiceGenerator.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<UserViewModel> ApplicationUsers { get; set; }
    }
}
