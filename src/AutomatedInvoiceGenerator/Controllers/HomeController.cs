using AutomatedInvoiceGenerator.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedInvoiceGenerator.Controllers
{
    [AssemblyVersionFilter]
    public class HomeController : Controller
    {
        public IActionResult Customers()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public IActionResult Invoices()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public IActionResult Export()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }

        public IActionResult Error()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            return View();
        }
    }
}
