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
    [Route("api")]
    public class ServiceItemsSetsApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceItemsSetsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceItemsSets
        [HttpGet("ServiceItemsSets")]
        public async Task<IActionResult> Get()
        {
            return Json(Mapper.Map<IEnumerable<ServiceItemsSetDto>>(await _context.ServiceItemsSets.ToListAsync()));
        }

        // GET api/ServiceItemsSets/5
        [HttpGet("ServiceItemsSets/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Json(Mapper.Map<ServiceItemsSetDto>((await _context.ServiceItemsSets.Where(g => g.Id == id).ToListAsync()).First()));
        }

        // GET: api/ServiceItemsSetsByCustomer/5
        [HttpGet("ServiceItemsSetsByCustomer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(int customerId)
        {
            var serviceItemsSets = await _context.ServiceItemsSets
                .Where(g => g.CustomerId == customerId)
                .OrderBy(o => o.Id)
                .ToListAsync();

            return Json(Mapper.Map<IEnumerable<ServiceItemsSetDto>>(serviceItemsSets));
        }

        // POST api/ServiceItemsSets
        [HttpPost("ServiceItemsSets")]
        public async Task<IActionResult> Post([FromBody]ServiceItemsSetDto newServiceItemsSetDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newServiceItemsSet = Mapper.Map<ServiceItemsSet>(newServiceItemsSetDto);

            _context.ServiceItemsSets.Add(newServiceItemsSet);
            await _context.SaveChangesAsync();

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

            updatedServiceItemsSet.CustomerId = updatedServiceItemsSetDto.CustomerId;

            _context.ServiceItemsSets.Update(updatedServiceItemsSet);
            await _context.SaveChangesAsync();

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

                var serviceItemsSetRelatedServiceItems = await _context.ServiceItems.Where(g => g.Id == serviceItemsSetToBeDeleted.Id).ToListAsync();

                if (serviceItemsSetRelatedServiceItems.Any())
                    return BadRequest("Zestaw usług zawiera powiązane usługi.");

                _context.ServiceItemsSets.Remove(serviceItemsSetToBeDeleted);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }

            return NotFound();
        }
    }
}
