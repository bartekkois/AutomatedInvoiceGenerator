using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutomatedInvoiceGenerator.Data;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutomatedInvoiceGenerator.DTO;
using AutomatedInvoiceGenerator.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using Microsoft.Extensions.Caching.Memory;

namespace AutomatedInvoiceGenerator.Controllers.API
{
    [Authorize]
    [Route("api")]
    public class ServiceItemsSetsApiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public ServiceItemsSetsApiController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: api/ServiceItemsSets
        [HttpGet("ServiceItemsSets")]
        public async Task<IActionResult> Get()
        {
            var serviceItemsSets = await _context.ServiceItemsSets.ToListAsync();

            if (!serviceItemsSets.Any())
                return Json(Mapper.Map<IEnumerable<ServiceItemsSetDto>>(Enumerable.Empty<ServiceItemsSet>()));

            return Json(Mapper.Map<IEnumerable<ServiceItemsSetDto>>(serviceItemsSets));
        }

        // GET api/ServiceItemsSets/5
        [HttpGet("ServiceItemsSets/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var serviceItemsSets = await _context.ServiceItemsSets.Where(g => g.Id == id).ToListAsync();

            if (!serviceItemsSets.Any())
                return Json(Mapper.Map<IEnumerable<ServiceItemsSetDto>>(Enumerable.Empty<ServiceItemsSet>()));

            return Json(Mapper.Map<ServiceItemsSetDto>(serviceItemsSets.First()));
        }

        // GET: api/ServiceItemsSetsByCustomer/5
        [HttpGet("ServiceItemsSetsByCustomer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var serviceItemsSets = await _context.ServiceItemsSets
                .Where(g => g.CustomerId == customerId)
                .OrderBy(o => o.Id)
                .ToListAsync();

            if (!serviceItemsSets.Any())
                return Json(Mapper.Map<IEnumerable<ServiceItemsSetDto>>(Enumerable.Empty<ServiceItemsSet>()));

            return Json(Mapper.Map<IEnumerable<ServiceItemsSetDto>>(serviceItemsSets));
        }

        // POST api/ServiceItemsSets
        [HttpPost("ServiceItemsSets")]
        public async Task<IActionResult> Post([FromBody]ServiceItemsSetDto newServiceItemsSetDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newServiceItemsSet = Mapper.Map<ServiceItemsSet>(newServiceItemsSetDto);

            try
            {
                _context.ServiceItemsSets.Add(newServiceItemsSet);
                await _context.SaveChangesAsync();
                _cache.Remove(IMemoryCacheKeys.customersCacheKey);
            }
            catch (Exception exception)
            {
                BadRequest(exception);
            }

            return CreatedAtRoute("", new { id = newServiceItemsSet.Id }, Mapper.Map<ServiceItemsSetDto>(newServiceItemsSet));
        }

        // PUT api/ServiceItemsSets/5
        [HttpPut("ServiceItemsSets/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]ServiceItemsSetDto updatedServiceItemsSetDto)
        {
            if (!ModelState.IsValid || updatedServiceItemsSetDto.Id != id)
                return BadRequest();

            var serviceItemsSets = await _context.ServiceItemsSets.Where(g => g.Id == id).ToListAsync();

            if (!serviceItemsSets.Any())
                return NotFound();

            var updatedServiceItemsSet = serviceItemsSets.First();
            Mapper.Map(updatedServiceItemsSetDto, updatedServiceItemsSet);

            try
            {
                _context.ServiceItemsSets.Update(updatedServiceItemsSet);
                await _context.SaveChangesAsync();
                _cache.Remove(IMemoryCacheKeys.customersCacheKey);
            }
            catch (Exception exception)
            {
                BadRequest(exception);
            }

            return new NoContentResult();
        }

        // DELETE api/ServiceItemsSets/5
        [HttpDelete("ServiceItemsSets/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var serviceItemsSet = await _context.ServiceItemsSets.Where(g => g.Id == id).ToListAsync();

            if (serviceItemsSet.Any())
            {
                var serviceItemsSetToBeDeleted = serviceItemsSet.First();

                var serviceItemsSetRelatedSubscriptionServiceItems = await _context.SubscriptionServiceItems.Where(g => g.ServiceItemsSetId == serviceItemsSetToBeDeleted.Id).ToListAsync();
                var serviceItemsSetRelatedOneTimeServiceItems = await _context.OneTimeServiceItems.Where(g => g.ServiceItemsSetId == serviceItemsSetToBeDeleted.Id).ToListAsync();

                if (serviceItemsSetRelatedSubscriptionServiceItems.Any() || serviceItemsSetRelatedOneTimeServiceItems.Any())
                    return BadRequest("Zestaw usług zawiera powiązane usługi.");

                try
                {
                    _context.ServiceItemsSets.Remove(serviceItemsSetToBeDeleted);
                    await _context.SaveChangesAsync();
                    _cache.Remove(IMemoryCacheKeys.customersCacheKey);
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
