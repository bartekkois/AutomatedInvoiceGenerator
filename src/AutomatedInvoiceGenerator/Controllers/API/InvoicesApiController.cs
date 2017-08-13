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
                .Where(d => d.InvoiceDate.Date >= startDate.Date && d.InvoiceDate.Date <= endDate.Date)
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

        // POST api/GenerateInvoice
        [HttpPost("GenerateInvoice")]
        public async Task<IActionResult> GenerateInvoice([FromBody]GenerateInvoiceDto generateInvoiceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            _logger.LogInformation("=== Generowanie zestawu faktur z datą: " + generateInvoiceDto.InvoiceDate);

            foreach (int customerId in generateInvoiceDto.Customers)
            {
                var customers = await _context.Customers.Where(c => c.Id == customerId).ToListAsync();

                if (!customers.Any())
                {
                    _logger.LogError("== Nie znaleziono abonenta o Id: " + customerId);
                    continue;
                }

                try
                {
                    await _generateInvoiceService.GenerateInvoice(customers.First(), generateInvoiceDto.StartDate, generateInvoiceDto.InvoiceDate);
                }
                catch (Exception exception)
                {
                    _logger.LogError("== Wystąpił błąd podczas generowania faktury: " + exception);
                }
            }

            return StatusCode(201);
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
