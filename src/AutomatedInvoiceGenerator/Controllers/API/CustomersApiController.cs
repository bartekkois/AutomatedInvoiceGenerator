﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutomatedInvoiceGenerator.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutomatedInvoiceGenerator.DTO;
using AutomatedInvoiceGenerator.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.Extensions.Caching.Memory;

namespace AutomatedInvoiceGenerator.Controllers.API
{
    [Authorize]
    [Route("api")]
    public class CustomersApiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public CustomersApiController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: api/Customers
        [HttpGet("Customers")]
        public async Task<IActionResult> Get()
        {
            if (!_cache.TryGetValue(IMemoryCacheKeys.customersCacheKey, out IList<Customer> cacheCustomersEntry))
            {
                cacheCustomersEntry = await _context.Customers
                    .Include(s => s.ServiceItemsSets)
                        .ThenInclude(i => i.OneTimeServiceItems)
                            .ThenInclude(j => j.InvoiceItems)
                    .Include(s => s.ServiceItemsSets)
                        .ThenInclude(i => i.SubscriptionServiceItems)
                            .ThenInclude(j => j.InvoiceItems)
                    .OrderBy(o => o.IsArchived)
                    .ThenBy(o => o.CustomerCode)
                    .ToListAsync();

                _cache.Set(IMemoryCacheKeys.customersCacheKey, cacheCustomersEntry, new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(300)));
            }

            if (!cacheCustomersEntry.Any())
                return Json(Mapper.Map<IEnumerable<CustomerDto>>(Enumerable.Empty<Customer>()));

            return Json(Mapper.Map<IEnumerable<CustomerDto>>(cacheCustomersEntry));
        }

        // GET: api/CustomersShort
        [HttpGet("CustomersShort")]
        public async Task<IActionResult> GetShort()
        {
            var customers = await _context.Customers
                .Where(c => c.IsArchived == false)
                .OrderBy(o => o.CustomerCode)
                .ToListAsync();

            if (!customers.Any())
                return Json(Mapper.Map<IEnumerable<CustomerShortDto>>(Enumerable.Empty<Customer>()));

            return Json(Mapper.Map<IEnumerable<CustomerShortDto>>(customers));
        }

        // GET: api/CustomersByGroup/5
        [HttpGet("CustomersByGroup/{groupId}")]
        public async Task<IActionResult> GetByGroup(int groupId)
        {
            if (!_cache.TryGetValue(IMemoryCacheKeys.customersCacheKey, out IList<Customer> cacheCustomersEntry))
            {
                cacheCustomersEntry = await _context.Customers
                    .Include(s => s.ServiceItemsSets)
                        .ThenInclude(i => i.OneTimeServiceItems)
                            .ThenInclude(j => j.InvoiceItems)
                    .Include(s => s.ServiceItemsSets)
                        .ThenInclude(i => i.SubscriptionServiceItems)
                            .ThenInclude(j => j.InvoiceItems)
                    .OrderBy(o => o.IsArchived)
                    .ThenBy(o => o.CustomerCode)
                    .ToListAsync();
            }

            if (!cacheCustomersEntry.Where(g => g.GroupId == groupId).Any())
                return Json(Mapper.Map<IEnumerable<CustomerDto>>(Enumerable.Empty<Customer>()));

            return base.Json(Mapper.Map<IEnumerable<CustomerDto>>(cacheCustomersEntry.Where(g => g.GroupId == groupId)));
        }

        // GET api/Customers/5
        [HttpGet("Customers/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var customers = await _context.Customers
                .Where(g => g.Id == id)
                .Include(s => s.ServiceItemsSets)
                    .ThenInclude(i => i.OneTimeServiceItems)
                .Include(s => s.ServiceItemsSets)
                    .ThenInclude(i => i.SubscriptionServiceItems)
                .ToListAsync();

            if (!customers.Any())
                return Json(Mapper.Map<IEnumerable<CustomerDto>>(Enumerable.Empty<Customer>()));

            return Json(Mapper.Map<CustomerDto>(customers.First()));
        }

        // POST api/Customers
        [HttpPost("Customers")]
        public async Task<IActionResult> Post([FromBody]CustomerDto newCustomerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var customersWithCustomerCodes = await _context.Customers.Where(g => g.CustomerCode.ToLower() == newCustomerDto.CustomerCode.ToLower() && g.IsArchived == false).ToListAsync();

            if (customersWithCustomerCodes.Any())
                return BadRequest("Kontrahent o podanym kodzie już istnieje.");

            var newCustomer = Mapper.Map<Customer>(newCustomerDto);

            try
            {
                _context.Customers.Add(newCustomer);
                await _context.SaveChangesAsync();
                _cache.Remove(IMemoryCacheKeys.customersCacheKey);
            }
            catch (Exception exception)
            {
                BadRequest(exception);
            }

            return CreatedAtRoute("", new { id = newCustomer.Id }, Mapper.Map<CustomerDto>(newCustomer));
        }

        // PUT api/Customers/5
        [HttpPut("Customers/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]CustomerDto updatedCustomerDto)
        {
            if (!ModelState.IsValid || updatedCustomerDto.Id != id)
                return BadRequest();

            try
            {
                var updatedCustomer = await _context.Customers.SingleAsync(g => g.Id == id);

                Mapper.Map(updatedCustomerDto, updatedCustomer);
                _context.Entry(updatedCustomer).OriginalValues["RowVersion"] = updatedCustomerDto.RowVersion;

                _context.Customers.Update(updatedCustomer);
                await _context.SaveChangesAsync();
                _cache.Remove(IMemoryCacheKeys.customersCacheKey);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return Conflict(exception);
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }

            return new NoContentResult();
        }

        // DELETE api/Customers/5
        [HttpDelete("Customers/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customers = await _context.Customers.Where(g => g.Id == id).ToListAsync();

            if (customers.Any())
            {
                var customerToBeDeleted = customers.First();
                var customerRelatedCustomers = await _context.ServiceItemsSets.Where(g => g.CustomerId == customerToBeDeleted.Id).ToListAsync();

                if (customerRelatedCustomers.Any())
                    return BadRequest("Kontrahent zawiera powiązane zestawy usług.");

                try
                {
                    _context.Customers.Remove(customerToBeDeleted);
                    await _context.SaveChangesAsync();
                    _cache.Remove(IMemoryCacheKeys.customersCacheKey);
                }
                catch (Exception exception)
                {
                    return BadRequest(exception);
                }

                return new NoContentResult();
            }

            return NotFound();
        }
    }
}
