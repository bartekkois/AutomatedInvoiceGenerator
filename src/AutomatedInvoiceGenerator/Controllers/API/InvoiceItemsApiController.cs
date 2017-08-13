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
using Microsoft.Extensions.Logging;

namespace AutomatedInvoiceGenerator.Controllers.API
{
    [Authorize]
    [Route("api")]
    public class InvoiceItemsApiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<InvoicesApiController> _logger;

        public InvoiceItemsApiController(ApplicationDbContext context, ILogger<InvoicesApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/InvoiceItems/5
        [HttpGet("InvoiceItems/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var invoiceItems = await _context.InvoiceItems
                .Where(i => i.Id == id)
                .ToListAsync();

            if(!invoiceItems.Any())
                return Json(Mapper.Map<IEnumerable<InvoiceItemDto>>(Enumerable.Empty<InvoiceItem>()));

            return base.Json(Mapper.Map<InvoiceItemDto>((invoiceItems).First()));
        }

        // POST api/InvoiceItems
        [HttpPost("InvoiceItems")]
        public async Task<IActionResult> Post([FromBody]InvoiceItemDto newInvoiceItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newInvoiceItem = Mapper.Map<InvoiceItem>(newInvoiceItemDto);

            try
            {
                await _context.InvoiceItems.AddAsync(newInvoiceItem);
                await _context.SaveChangesAsync();
            }
            catch(Exception exception)
            {
                return BadRequest(exception);
            }

            return CreatedAtRoute("", new { id = newInvoiceItem.Id }, Mapper.Map<InvoiceItemDto>(newInvoiceItem));
        }

        // PUT api/InvoiceItems/5
        [HttpPut("InvoiceItems/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]InvoiceItemDto updatedInvoiceItemDto)
        {
            if (!ModelState.IsValid || updatedInvoiceItemDto.Id != id)
                return BadRequest();

            var invoiceItems = await _context.InvoiceItems.Where(g => g.Id == id).ToListAsync();

            if (!invoiceItems.Any())
                return NotFound();

            var updatedInvoiceItem = invoiceItems.First();
            Mapper.Map(updatedInvoiceItemDto, updatedInvoiceItem);

            try
            {
                _context.InvoiceItems.Update(updatedInvoiceItem);
                await _context.SaveChangesAsync();
            }
            catch(Exception exception)
            {
                return BadRequest(exception);
            }

            return new NoContentResult();
        }

        // DELETE api/InvoiceItems/5
        [HttpDelete("InvoiceItems/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var invoiceItems = await _context.InvoiceItems.Where(g => g.Id == id).ToListAsync();

            if (invoiceItems.Any())
            {
                var invoiceItemsToBeDeleted = invoiceItems.First();

                try
                {
                    _context.InvoiceItems.Remove(invoiceItemsToBeDeleted);
                    await _context.SaveChangesAsync();
                }
                catch (Exception exception)
                {
                    BadRequest(exception);
                }

                return new NoContentResult();
            }

            return NotFound();
        }
    }
}
