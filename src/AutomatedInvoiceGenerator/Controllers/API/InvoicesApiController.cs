﻿using System.Collections.Generic;
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

namespace AutomatedInvoiceGenerator.Controllers.API
{
    [Authorize]
    [Route("api")]
    public class InvoicesApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Invoices/5
        [HttpGet("Invoices/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var invoices = await _context.Invoices
                .Where(i => i.Id == id)
                .Include(o => o.InvoiceItems)
                .ToListAsync();

            if(!invoices.Any())
                return NotFound();

            return base.Json(Mapper.Map<InvoiceDto>((invoices).First()));
        }

        // GET api/Invoices/5
        [HttpGet("InvoicesByCustomer/{customerId}")] 
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var invoices = await _context.Invoices
                .Where(g => g.CustomerId == customerId)
                .Include(o => o.InvoiceItems)
                .OrderBy(o => o.InvoiceDate)
                .ToListAsync();

            if (!invoices.Any())
                return NotFound();

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
                await _context.InvoicesItems.AddRangeAsync(newInvoice.InvoiceItems);
                await _context.SaveChangesAsync();
            }
            catch(Exception exception)
            {
                return BadRequest(exception);
            }

            return CreatedAtRoute("", new { id = newInvoice.Id }, Mapper.Map<InvoiceDto>(newInvoice));
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

            try
            {
                var updatedInvoices = invoices.First();

                Mapper.Map(updatedInvoiceDto, updatedInvoices);
                _context.Invoices.Update(updatedInvoices);

                foreach (InvoiceItemDto invoiceItemDto in updatedInvoiceDto.InvoiceItemsDto)
                {
                    var relatedInvoiceItems = await _context.InvoicesItems.Where(g => g.Id == invoiceItemDto.Id).ToListAsync();

                    if (relatedInvoiceItems.Any())
                    {
                        InvoiceItem updatedInvoiceItem = relatedInvoiceItems.First();
                        Mapper.Map(invoiceItemDto, updatedInvoiceItem);
                        _context.InvoicesItems.Update(updatedInvoiceItem);
                    }
                    else
                    {
                        await _context.InvoicesItems.AddAsync(Mapper.Map<InvoiceItem>(invoiceItemDto));
                    }

                    await _context.SaveChangesAsync();
                }

                await _context.SaveChangesAsync();
            }
            catch(Exception exceptoin)
            {
                return BadRequest(exceptoin);
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
                    _context.InvoicesItems.RemoveRange(await _context.InvoicesItems.Where(g => g.InvoiceId == invoiceToBeDeleted.Id).ToListAsync());
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
