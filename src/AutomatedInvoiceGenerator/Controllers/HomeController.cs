using AutomatedInvoiceGenerator.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AutomatedInvoiceGenerator.Controllers
{
    [AssemblyVersionFilter]
    public class HomeController : Controller
    {
        public IActionResult Customers()
        {
            return View();
        }

        public IActionResult Invoices()
        {
            return View();
        }

        public IActionResult Export()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
