using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutomatedInvoiceGenerator.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutomatedInvoiceGenerator.DTO;
using AutomatedInvoiceGenerator.Models;

namespace AutomatedInvoiceGenerator.Controllers.API
{
    [Route("api")]
    public class CustomersApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet("Customers")]
        public async Task<IActionResult> Get()
        {
            var customers = await _context.Customers
                .Include(s => s.ServiceItemsSets)
                    .ThenInclude(i => i.OneTimeServiceItems)
                .Include(s => s.ServiceItemsSets)
                    .ThenInclude(i => i.SubscriptionServiceItems)
                .OrderBy(o => o.IsArchived)
                .ThenBy(o => o.CustomerCode)
                .ToListAsync();

            return Json(Mapper.Map<IEnumerable<CustomerDto>>(customers));
        }

        // GET: api/CustomersByGroup/5
        [HttpGet("CustomersByGroup/{groupId}")]
        public async Task<IActionResult> GetByGroup(int groupId)
        {
            var customers = await _context.Customers
                .Where(g => g.GroupId == groupId)
                .Include(s => s.ServiceItemsSets)
                    .ThenInclude(i => i.OneTimeServiceItems)
                .Include(s => s.ServiceItemsSets)
                    .ThenInclude(i => i.SubscriptionServiceItems)
                .OrderBy(o => o.IsArchived)
                .ThenBy(o => o.CustomerCode)
                .ToListAsync();

            return Json(Mapper.Map<IEnumerable<CustomerDto>>(customers));
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

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("", new { id = newCustomer.Id }, Mapper.Map<CustomerDto>(newCustomer));
        }

        // PUT api/Customers/5
        [HttpPut("Customers/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]CustomerDto updatedCustomerDto)
        {
            if (!ModelState.IsValid || updatedCustomerDto.Id != id)
                return BadRequest();

            var customers = await _context.Customers.Where(g => g.Id == id).ToListAsync();

            if (!customers.Any())
                return NotFound();

            Customer updatedCustomer = customers.First();
            updatedCustomer.CustomerCode = updatedCustomerDto.CustomerCode;
            updatedCustomer.Name = updatedCustomerDto.Name;
            updatedCustomer.BrandName = updatedCustomerDto.BrandName;
            updatedCustomer.Location = updatedCustomerDto.Location;
            updatedCustomer.Notes = updatedCustomerDto.Notes;
            updatedCustomer.InvoiceCustomerSpecificTag = updatedCustomerDto.InvoiceCustomerSpecificTag;
            updatedCustomer.InvoiceDelivery = updatedCustomerDto.InvoiceDelivery;
            updatedCustomer.PriceCalculation = updatedCustomerDto.PriceCalculation;
            updatedCustomer.PaymentMethod = updatedCustomerDto.PaymentMethod;
            updatedCustomer.PaymentPeriod = updatedCustomerDto.PaymentPeriod;
            updatedCustomer.IsVatEu = updatedCustomerDto.IsVatEu;
            updatedCustomer.IsBlocked = updatedCustomerDto.IsBlocked;
            updatedCustomer.IsSuspended = updatedCustomerDto.IsSuspended;
            updatedCustomer.IsArchived = updatedCustomerDto.IsArchived;
            updatedCustomer.GroupId = updatedCustomerDto.GroupId;

            _context.Customers.Update(updatedCustomer);
            await _context.SaveChangesAsync();

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

                _context.Customers.Remove(customerToBeDeleted);
                await _context.SaveChangesAsync();

                return new NoContentResult();
            }

            return NotFound();
        }
    }
}
