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
    [Route("api/OneTimeServiceItems")]
    public class OneTimeServiceItemsApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OneTimeServiceItemsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceItems
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Json(Mapper.Map<IEnumerable<OneTimeServiceItemDto>>(await _context.ServiceItems.OfType<OneTimeServiceItem>().Where(g => g.IsArchived == false).ToListAsync()));
        }

        // GET api/ServiceItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Json(Mapper.Map<OneTimeServiceItemDto>((await _context.ServiceItems.OfType<OneTimeServiceItem>().Where(g => g.Id == id && g.IsArchived == false).ToListAsync()).First()));
        }

        // POST api/ServiceItems
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OneTimeServiceItemDto newOneTimeServiceItemDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newOneTimeServiceItem = Mapper.Map<OneTimeServiceItem>(newOneTimeServiceItemDto);
            newOneTimeServiceItem.IsArchived = false;

            _context.ServiceItems.Add(newOneTimeServiceItem);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("", new { id = newOneTimeServiceItem.Id }, Mapper.Map<OneTimeServiceItemDto>(newOneTimeServiceItem));
        }

        // PUT api/ServiceItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]OneTimeServiceItemDto updatedOneTimeServiceItemDto)
        {
            if (!ModelState.IsValid || updatedOneTimeServiceItemDto.Id != id)
                return BadRequest();

            var oneTimeServiceItems = await _context.ServiceItems.OfType<OneTimeServiceItem>().Where(g => g.Id == id).ToListAsync();

            if (!oneTimeServiceItems.Any())
                return NotFound();

            var updatedOneTimeServiceItem = oneTimeServiceItems.First();

            updatedOneTimeServiceItem.ServiceCategoryType = updatedOneTimeServiceItemDto.ServiceCategoryType;
            updatedOneTimeServiceItem.RemoteSystemServiceCode = updatedOneTimeServiceItemDto.RemoteSystemServiceCode;
            updatedOneTimeServiceItem.Name = updatedOneTimeServiceItemDto.Name;
            updatedOneTimeServiceItem.SubName = updatedOneTimeServiceItemDto.SubName;
            updatedOneTimeServiceItem.IsSubNamePrinted = updatedOneTimeServiceItemDto.IsSubNamePrinted;
            updatedOneTimeServiceItem.SpecificLocation = updatedOneTimeServiceItemDto.SpecificLocation;
            updatedOneTimeServiceItem.ServiceItemCustomerSpecificTag = updatedOneTimeServiceItemDto.ServiceItemCustomerSpecificTag;
            updatedOneTimeServiceItem.Notes = updatedOneTimeServiceItemDto.Notes;
            updatedOneTimeServiceItem.IsValueVariable = updatedOneTimeServiceItemDto.IsValueVariable;
            updatedOneTimeServiceItem.NetValue = updatedOneTimeServiceItemDto.NetValue;
            updatedOneTimeServiceItem.Quantity = updatedOneTimeServiceItemDto.Quantity;
            updatedOneTimeServiceItem.VATRate = updatedOneTimeServiceItemDto.VATRate;
            updatedOneTimeServiceItem.GrossValueAdded = updatedOneTimeServiceItemDto.GrossValueAdded;
            updatedOneTimeServiceItem.IsManual = updatedOneTimeServiceItemDto.IsManual;
            updatedOneTimeServiceItem.IsBlocked = updatedOneTimeServiceItemDto.IsBlocked;
            updatedOneTimeServiceItem.IsSuspended = updatedOneTimeServiceItemDto.IsSuspended;
            updatedOneTimeServiceItem.InstallationDate = updatedOneTimeServiceItemDto.InstallationDate;
            updatedOneTimeServiceItem.IsInvoiced = updatedOneTimeServiceItemDto.IsInvoiced;
            updatedOneTimeServiceItem.IsArchived = updatedOneTimeServiceItemDto.IsArchived;
            updatedOneTimeServiceItem.ServiceItemsSetId = updatedOneTimeServiceItemDto.ServiceItemsSetId;

            _context.ServiceItems.Update(updatedOneTimeServiceItem);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        // DELETE api/ServiceItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var oneTimeserviceItems = await _context.ServiceItems.OfType<OneTimeServiceItem>().Where(g => g.Id == id).ToListAsync();

            if (oneTimeserviceItems.Any())
            {
                var oneTimeServiceItemToBeDeleted = oneTimeserviceItems.First();

                _context.ServiceItems.Remove(oneTimeServiceItemToBeDeleted);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }

            return NotFound();
        }
    }
}
