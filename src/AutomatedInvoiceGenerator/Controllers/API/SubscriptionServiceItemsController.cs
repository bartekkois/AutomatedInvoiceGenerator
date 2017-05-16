using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutomatedInvoiceGenerator.Data;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using AutomatedInvoiceGenerator.DTO;
using AutomatedInvoiceGenerator.Models;

namespace AutomatedInvoiceGenerator.Controllers.API
{
    [Route("api/SubscriptionServiceItems")]
    public class SubscriptionServiceItemsApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionServiceItemsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceItems
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(Mapper.Map<IEnumerable<SubscriptionServiceItemDto>>(await _context.ServiceItems.OfType<SubscriptionServiceItem>().Where(g => g.IsArchived == false).ToListAsync()));
        }

        // GET api/ServiceItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Json(Mapper.Map<SubscriptionServiceItemDto>((await _context.ServiceItems.OfType<SubscriptionServiceItem>().Where(g => g.Id == id && g.IsArchived == false).ToListAsync()).First()));
        }

        // POST api/ServiceItems
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SubscriptionServiceItemDto newSubscriptionServiceItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newSubscriptionServiceItem = Mapper.Map<SubscriptionServiceItem>(newSubscriptionServiceItemDto);
            newSubscriptionServiceItem.IsArchived = false;

            _context.ServiceItems.Add(newSubscriptionServiceItem);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("", new { id = newSubscriptionServiceItem.Id }, Mapper.Map<SubscriptionServiceItemDto>(newSubscriptionServiceItem));
        }

        // PUT api/ServiceItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]SubscriptionServiceItemDto updatedSubscriptionServiceItemDto)
        {
            if (!ModelState.IsValid || updatedSubscriptionServiceItemDto.Id != id)
                return BadRequest();

            var subscriptionServiceItems = await _context.ServiceItems.OfType<SubscriptionServiceItem>().Where(g => g.Id == id).ToListAsync();

            if (!subscriptionServiceItems.Any())
                return NotFound();

            var updatedSubscriptionServiceItem = subscriptionServiceItems.First();

            updatedSubscriptionServiceItem.ServiceCategoryType = updatedSubscriptionServiceItemDto.ServiceCategoryType;
            updatedSubscriptionServiceItem.RemoteSystemServiceCode = updatedSubscriptionServiceItemDto.RemoteSystemServiceCode;
            updatedSubscriptionServiceItem.Name = updatedSubscriptionServiceItemDto.Name;
            updatedSubscriptionServiceItem.SubName = updatedSubscriptionServiceItemDto.SubName;
            updatedSubscriptionServiceItem.IsSubNamePrinted = updatedSubscriptionServiceItemDto.IsSubNamePrinted;
            updatedSubscriptionServiceItem.SpecificLocation = updatedSubscriptionServiceItemDto.SpecificLocation;
            updatedSubscriptionServiceItem.ServiceItemCustomerSpecificTag = updatedSubscriptionServiceItemDto.ServiceItemCustomerSpecificTag;
            updatedSubscriptionServiceItem.Notes = updatedSubscriptionServiceItemDto.Notes;
            updatedSubscriptionServiceItem.IsValueVariable = updatedSubscriptionServiceItemDto.IsValueVariable;
            updatedSubscriptionServiceItem.NetValue = updatedSubscriptionServiceItemDto.NetValue;
            updatedSubscriptionServiceItem.Quantity = updatedSubscriptionServiceItemDto.Quantity;
            updatedSubscriptionServiceItem.VATRate = updatedSubscriptionServiceItemDto.VATRate;
            updatedSubscriptionServiceItem.GrossValueAdded = updatedSubscriptionServiceItemDto.GrossValueAdded;
            updatedSubscriptionServiceItem.IsManual = updatedSubscriptionServiceItemDto.IsManual;
            updatedSubscriptionServiceItem.IsBlocked = updatedSubscriptionServiceItemDto.IsBlocked;
            updatedSubscriptionServiceItem.IsSuspended = updatedSubscriptionServiceItemDto.IsSuspended;
            updatedSubscriptionServiceItem.StartDate = updatedSubscriptionServiceItemDto.StartDate;
            updatedSubscriptionServiceItem.EndDate = updatedSubscriptionServiceItemDto.EndDate;
            updatedSubscriptionServiceItem.IsArchived = updatedSubscriptionServiceItemDto.IsArchived;

            _context.ServiceItems.Update(updatedSubscriptionServiceItem);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        // DELETE api/ServiceItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var subscriptionServiceItems = await _context.ServiceItems.OfType<SubscriptionServiceItem>().Where(g => g.Id == id).ToListAsync();

            if (subscriptionServiceItems.Any())
            {
                var subscriptionServiceItemToBeDeleted = subscriptionServiceItems.First();

                _context.ServiceItems.Remove(subscriptionServiceItemToBeDeleted);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }

            return NotFound();
        }
    }
}
