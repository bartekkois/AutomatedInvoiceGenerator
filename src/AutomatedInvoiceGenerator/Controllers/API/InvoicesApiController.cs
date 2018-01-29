using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutomatedInvoiceGenerator.Data;
using System.Threading.Tasks;
using AutoMapper;
using AutomatedInvoiceGenerator.DTO;
using AutomatedInvoiceGenerator.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using AutomatedInvoiceGenerator.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;

namespace AutomatedInvoiceGenerator.Controllers.API
{
    [Authorize]
    [Route("api")]
    public class InvoicesApiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InvoicesApiController> _logger;
        private readonly IGenerateInvoiceService _generateInvoiceService;

        public InvoicesApiController(ApplicationDbContext context, ILogger<InvoicesApiController> logger, IGenerateInvoiceService generateInvoiceService)
        {
            _context = context;
            _logger = logger;
            _generateInvoiceService = generateInvoiceService;
        }

        // GET: api/Invoices/5
        [HttpGet("Invoices/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var invoices = await _context.Invoices
                .Where(i => i.Id == id)
                .Include(o => o.Customer)
                .Include(o => o.InvoiceItems)
                .ToListAsync();

            if(!invoices.Any())
                return Json(Mapper.Map<IEnumerable<InvoiceDto>>(Enumerable.Empty<Invoice>()));

            return base.Json(Mapper.Map<InvoiceDto>((invoices).First()));
        }

        // GET api/InvoicesByDateAndCustomer/2017-07-01T00:00:00/2017-07-31T00:00:00/5
        [HttpGet("InvoicesByDateAndCustomer/{startDate:datetime}/{endDate:datetime}/{customerId?}")] 
        public async Task<IActionResult> GetByDateAndCustomer(DateTime startDate, DateTime endDate, int? customerId)
        {
            var invoices = await _context.Invoices
                .Where(d => d.InvoiceDate.Date >= startDate.ToLocalTime().Date && d.InvoiceDate.ToLocalTime().Date <= endDate.Date)
                .Where(g => !customerId.HasValue || g.CustomerId == customerId)
                .Include(i => i.Customer)
                .Include(i => i.InvoiceItems)
                .OrderBy(o => o.Customer.CustomerCode)
                .OrderBy(o => o.InvoiceDate)
                .ToListAsync();

            if (!invoices.Any())
                return Json(Mapper.Map<IEnumerable<InvoiceDto>>(Enumerable.Empty<Invoice>()));

            return base.Json(Mapper.Map<IEnumerable<InvoiceDto>>(invoices));
        }

        // POST api/Invoices
        [HttpPost("Invoices")]
        public async Task<IActionResult> Post([FromBody]InvoiceDto newInvoiceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newInvoice = Mapper.Map<Invoice>(newInvoiceDto);

            try
            {
                await _context.Invoices.AddAsync(newInvoice);
                await _context.SaveChangesAsync();
            }
            catch(Exception exception)
            {
                return BadRequest(exception);
            }

            return CreatedAtRoute("", new { id = newInvoice.Id }, Mapper.Map<InvoiceDto>(newInvoice));
        }

        // POST api/GenerateInvoices
        [HttpPost("GenerateInvoices")]
        public async Task<IActionResult> GenerateInvoices([FromBody]GenerateInvoicesDto generateInvoiceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _logger.LogInformation("=== Generowanie zestawu faktur z datą: " + generateInvoiceDto.InvoiceDate.ToLocalTime().Date + " od dnia: " + generateInvoiceDto.StartDate.ToLocalTime().Date);

            foreach (Customer customer in _context.Customers.Where(c => c.IsArchived == false).ToList())
            {
                try
                {
                    await _generateInvoiceService.GenerateInvoice(customer, generateInvoiceDto.StartDate.ToLocalTime(), generateInvoiceDto.InvoiceDate.ToLocalTime());
                }
                catch (Exception exception)
                {
                    _logger.LogError("== Wystąpił błąd podczas generowania faktury: " + exception);
                }
            }

            return StatusCode(201);
        }

        // GET api/GenerateInvoicesLogs/2017-07-01T00:00:00
        [HttpGet("GenerateInvoicesLogs/{logsDate:datetime}")]
        public async Task<IActionResult> GenerateInvoiceLogs(DateTime logsDate)
        {
            var logsFilePath = "Logs/GenerateInvoices/log-" + logsDate.ToString("yyyy") + logsDate.ToString("MM") + logsDate.ToString("dd") + ".txt";

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

        // PUT api/Invoices/5
        [HttpPut("Invoices/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]InvoiceDto updatedInvoiceDto)
        {
            if (!ModelState.IsValid || updatedInvoiceDto.Id != id)
                return BadRequest();

            var invoices = await _context.Invoices.Where(g => g.Id == id).ToListAsync();

            if (!invoices.Any())
                return NotFound();

            var updatedInvoices = invoices.First();
            Mapper.Map(updatedInvoiceDto, updatedInvoices);

            try
            {
                _context.Invoices.Update(updatedInvoices);
                await _context.SaveChangesAsync();
            }
            catch(Exception exception)
            {
                return BadRequest(exception);
            }

            return new NoContentResult();
        }

        // DELETE api/Invoices/5
        [HttpDelete("Invoices/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if ((await _context.Invoices.Where(g => g.Id == id).ToListAsync()).Any())
            {
                var invoiceToBeDeleted = (await _context.Invoices.Where(g => g.Id == id).ToListAsync()).First();

                try
                {
                    _context.InvoiceItems.RemoveRange(await _context.InvoiceItems.Where(g => g.InvoiceId == invoiceToBeDeleted.Id).ToListAsync());
                    _context.Invoices.Remove(invoiceToBeDeleted);
                    await _context.SaveChangesAsync();
                }
                catch(Exception exception)
                {
                    return BadRequest(exception);
                }

                return new NoContentResult();
            }

            return NotFound();
        }
    }
}
