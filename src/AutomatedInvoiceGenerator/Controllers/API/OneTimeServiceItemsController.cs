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
    [Route("api/OneTimeServiceItems")]
    public class OneTimeServiceItemsApiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public OneTimeServiceItemsApiController(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: api/OneTimeServiceItems
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var oneTimeServiceItems = await _context.ServiceItems.OfType<OneTimeServiceItem>().OrderBy(o => o.IsArchived).ToListAsync();

            if (!oneTimeServiceItems.Any())
                return Json(Mapper.Map<IEnumerable<OneTimeServiceItemDto>>(Enumerable.Empty<OneTimeServiceItem>()));

            return Json(Mapper.Map<IEnumerable<OneTimeServiceItemDto>>(oneTimeServiceItems));
        }

        // GET api/OneTimeServiceItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var oneTimeServiceItems = await _context.ServiceItems.OfType<OneTimeServiceItem>().Where(g => g.Id == id).OrderBy(o => o.IsArchived).ToListAsync();

            if (!oneTimeServiceItems.Any())
                return Json(Mapper.Map<IEnumerable<OneTimeServiceItemDto>>(Enumerable.Empty<OneTimeServiceItem>()));

            return Json(Mapper.Map<OneTimeServiceItemDto>(oneTimeServiceItems.First()));
        }

        // POST api/OneTimeServiceItems
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OneTimeServiceItemDto newOneTimeServiceItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newOneTimeServiceItem = Mapper.Map<OneTimeServiceItem>(newOneTimeServiceItemDto);
            newOneTimeServiceItem.IsArchived = false;

            try
            {
                _context.ServiceItems.Add(newOneTimeServiceItem);
                await _context.SaveChangesAsync();
                _cache.Remove(IMemoryCacheKeys.customersCacheKey);
            }
            catch (Exception exception)
            {
                BadRequest(exception);
            }

            return CreatedAtRoute("", new { id = newOneTimeServiceItem.Id }, Mapper.Map<OneTimeServiceItemDto>(newOneTimeServiceItem));
        }

        // PUT api/OneTimeServiceItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]OneTimeServiceItemDto updatedOneTimeServiceItemDto)
        {
            if (!ModelState.IsValid || updatedOneTimeServiceItemDto.Id != id)
                return BadRequest();

            try
            {
                var updatedOneTimeServiceItem = await _context.ServiceItems.OfType<OneTimeServiceItem>().SingleAsync(g => g.Id == id);

                Mapper.Map(updatedOneTimeServiceItemDto, updatedOneTimeServiceItem);
                _context.Entry(updatedOneTimeServiceItem).OriginalValues["RowVersion"] = updatedOneTimeServiceItemDto.RowVersion;

                _context.ServiceItems.Update(updatedOneTimeServiceItem);
                await _context.SaveChangesAsync();
                _cache.Remove(IMemoryCacheKeys.customersCacheKey);
            }
            catch(DbUpdateConcurrencyException exception)
            {
                Conflict(exception);
            }
            catch (Exception exception)
            {
                BadRequest(exception);
            }

            return new NoContentResult();
        }

        // DELETE api/OneTimeServiceItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var oneTimeserviceItems = await _context.ServiceItems.OfType<OneTimeServiceItem>().Where(g => g.Id == id).ToListAsync();

            if (oneTimeserviceItems.Any())
            {
                var oneTimeServiceItemToBeDeleted = oneTimeserviceItems.First();

                try
                {
                    _context.ServiceItems.Remove(oneTimeServiceItemToBeDeleted);
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
