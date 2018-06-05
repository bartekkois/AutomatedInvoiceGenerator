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
    [Route("api/SubscriptionServiceItems")]
    public class SubscriptionServiceItemsApiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public SubscriptionServiceItemsApiController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: api/SubscriptionServiceItems
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var subscriptionTimeServiceItems = await _context.ServiceItems.OfType<SubscriptionServiceItem>().OrderBy(o => o.IsArchived).ToListAsync();

            if (!subscriptionTimeServiceItems.Any())
                return Json(Mapper.Map<IEnumerable<SubscriptionServiceItemDto>>(Enumerable.Empty<SubscriptionServiceItem>()));

            return Json(Mapper.Map<IEnumerable<SubscriptionServiceItemDto>>(subscriptionTimeServiceItems));
        }

        // GET api/SubscriptionServiceItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var subscriptionTimeServiceItems = await _context.ServiceItems.OfType<SubscriptionServiceItem>().Where(g => g.Id == id).OrderBy(o => o.IsArchived).ToListAsync();

            if (!subscriptionTimeServiceItems.Any())
                return Json(Mapper.Map<IEnumerable<SubscriptionServiceItemDto>>(Enumerable.Empty<SubscriptionServiceItem>()));

            return Json(Mapper.Map<SubscriptionServiceItemDto>(subscriptionTimeServiceItems.First()));
        }

        // POST api/SubscriptionServiceItems
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SubscriptionServiceItemDto newSubscriptionServiceItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newSubscriptionServiceItem = Mapper.Map<SubscriptionServiceItem>(newSubscriptionServiceItemDto);
            newSubscriptionServiceItem.IsArchived = false;

            try
            {
                _context.ServiceItems.Add(newSubscriptionServiceItem);
                await _context.SaveChangesAsync();
                _cache.Remove(IMemoryCacheKeys.customersCacheKey);
            }
            catch (Exception exception)
            {
                BadRequest(exception);
            }

            return CreatedAtRoute("", new { id = newSubscriptionServiceItem.Id }, Mapper.Map<SubscriptionServiceItemDto>(newSubscriptionServiceItem));
        }

        // PUT api/SubscriptionServiceItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]SubscriptionServiceItemDto updatedSubscriptionServiceItemDto)
        {
            if (!ModelState.IsValid || updatedSubscriptionServiceItemDto.Id != id)
                return BadRequest();

            var subscriptionServiceItems = await _context.ServiceItems.OfType<SubscriptionServiceItem>().Where(g => g.Id == id).ToListAsync();

            if (!subscriptionServiceItems.Any())
                return NotFound();

            var updatedSubscriptionServiceItem = subscriptionServiceItems.First();
            Mapper.Map(updatedSubscriptionServiceItemDto, updatedSubscriptionServiceItem);

            try
            {
                _context.ServiceItems.Update(updatedSubscriptionServiceItem);
                await _context.SaveChangesAsync();
                _cache.Remove(IMemoryCacheKeys.customersCacheKey);
            }
            catch (Exception exception)
            {
                BadRequest(exception);
            }

            return new NoContentResult();
        }

        // DELETE api/SubscriptionServiceItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var subscriptionServiceItems = await _context.ServiceItems.OfType<SubscriptionServiceItem>().Where(g => g.Id == id).ToListAsync();

            if (subscriptionServiceItems.Any())
            {
                var subscriptionServiceItemToBeDeleted = subscriptionServiceItems.First();

                try
                {
                    _context.ServiceItems.Remove(subscriptionServiceItemToBeDeleted);
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
