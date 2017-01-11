using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace AutomatedInvoiceGenerator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Customers()
        {
            ViewData["Version"] = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            return View();
        }

        public IActionResult Invoices()
        {
            ViewData["Version"] = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            return View();
        }

        public IActionResult Export()
        {
            ViewData["Version"] = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
