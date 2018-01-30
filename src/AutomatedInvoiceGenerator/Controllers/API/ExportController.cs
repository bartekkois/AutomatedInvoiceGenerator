using AutomatedInvoiceGenerator.Data;
using AutomatedInvoiceGenerator.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedInvoiceGenerator.Controllers
{
    [Authorize]
    [Route("api")]
    public class ExportController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IExportService _exportService;
        private readonly ILogger<ExportController> _logger;

        public ExportController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context, ILogger<ExportController> logger, IExportService exportService)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
            _logger = logger;
            _exportService = exportService;
        }

        // GET: api/ExportInvoicesToComarchOptimaXMLFormatArchive/2017-07-01T00:00:00/2017-07-31T00:00:00
        [HttpGet("ExportInvoicesToComarchOptimaXMLFormatArchive/{exportStartDate:datetime}/{exportEndDate:datetime}")]
        public async Task<IActionResult> ExportInvoicesToComarchOptimaXMLFormatArchiveAsync(DateTime exportStartDate, DateTime exportEndDate)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid && exportEndDate.ToLocalTime() < exportStartDate.ToLocalTime())
                return BadRequest();

            try
            {
                string temporaryXMLDirectory = _hostingEnvironment.ContentRootPath + "/Temp/XML/";
                string temporaryZIPDirectory = _hostingEnvironment.ContentRootPath + "/Temp/ZIP/";
                string temporaryZIPFile = "OptimaXMLExportedInvoices - " + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".zip";

                await Task.Run(() =>
                {
                    _logger.LogInformation("=== Rozpoczecie eksportu faktur z zakresu: " + exportStartDate.ToLocalTime() + " - " + exportEndDate.ToLocalTime());
                    _exportService.FlushOrCreateDirectory(temporaryXMLDirectory);
                    _exportService.FlushOrCreateDirectory(temporaryZIPDirectory);
                    _exportService.ExportInvoicesToComarchOptimaXMLFormat(exportStartDate.ToLocalTime(), exportEndDate.ToLocalTime(), temporaryXMLDirectory);
                    _exportService.CreateZipArchive(temporaryXMLDirectory, temporaryZIPDirectory + temporaryZIPFile);
                    _logger.LogInformation("=== Zakończenie eksportu faktur z zakresu: " + exportStartDate.ToLocalTime() + " - " + exportEndDate.ToLocalTime());
                });

                return PhysicalFile(temporaryZIPDirectory + temporaryZIPFile, "application/zip", temporaryZIPFile);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                return BadRequest(exception);
            }
        }

        // GET api/ExportInvoicesToComarchOptimaXMLFormatArchiveLogs/2017-07-01T00:00:00
        [HttpGet("ExportInvoicesToComarchOptimaXMLFormatArchiveLogs/{logsDate:datetime}")]
        public async Task<IActionResult> ExportInvoicesToComarchOptimaXMLFormatArchiveLogs(DateTime logsDate)
        {
            var logsFilePath = "Logs/ExportInvoices/log-" + logsDate.ToString("yyyy") + logsDate.ToString("MM") + logsDate.ToString("dd") + ".txt";

            if (logsDate.ToLocalTime().Date > DateTime.Now.ToLocalTime().Date)
                return BadRequest();

            if (!System.IO.File.Exists(logsFilePath))
                return NotFound();

            try
            {
                StringBuilder logs = new StringBuilder();
                using (var stream = new FileStream(logsFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        logs.Append(line + "\n");
                    }
                }

                return Content(logs.ToString());
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }
        }
    }
}
