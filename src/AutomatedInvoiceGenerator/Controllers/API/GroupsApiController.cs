using Microsoft.AspNetCore.Mvc;
using AutomatedInvoiceGenerator.Data;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;
using AutomatedInvoiceGenerator.DTO;
using AutomatedInvoiceGenerator.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;

namespace AutomatedInvoiceGenerator.Controllers.API
{
    [Authorize]
    [Route("api/Groups")]
    public class GroupsApiController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var groups = await _context.Groups.Where(g => g.IsArchived == false).OrderBy(o => o.Name).ToListAsync();

            if (!groups.Any())
                return NotFound();

            return base.Json(Mapper.Map<IEnumerable<GroupDto>>(groups));
        }

        // GET api/Groups/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var groups = await _context.Groups.Where(g => g.Id == id && g.IsArchived == false).ToListAsync();

            if (!groups.Any())
                return NotFound();

            return base.Json(Mapper.Map<GroupDto>(groups.First())); 
        }

        // POST api/Groups
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]GroupDto newGroupDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var groupsWithIdenticalName = await _context.Groups.Where(g => g.Name.ToLower() == newGroupDto.Name.ToLower() && g.IsArchived == false).ToListAsync();

            if (groupsWithIdenticalName.Any())
                return BadRequest("Grupa o podanej nazwie już istnieje.");

            var newGroup = Mapper.Map<Group>(newGroupDto);
            newGroup.IsArchived = false;

            try
            {
                _context.Groups.Add(newGroup);
                await _context.SaveChangesAsync();
            }
            catch(Exception exception)
            {
                return BadRequest(exception);
            }

            return CreatedAtRoute("", new { id = newGroup.Id }, Mapper.Map<GroupDto>(newGroup));
        }

        // PUT api/Groups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]GroupDto updatedGroupDto)
        {
            if (!ModelState.IsValid || updatedGroupDto.Id != id)
                return BadRequest();

            var groups = await _context.Groups.Where(g => g.Id == id).ToListAsync();

            if (!groups.Any())
                return NotFound();

            var updatedGroup = groups.First();

            Mapper.Map(updatedGroupDto, updatedGroup);

            try
            { 
                _context.Groups.Update(updatedGroup);
                await _context.SaveChangesAsync();
            }
            catch (Exception exception)
            {
                return BadRequest(exception);
            }

            return new NoContentResult();
        }

        // DELETE api/Groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var groups = await _context.Groups.Where(g => g.Id == id).ToListAsync();

            if (groups.Any())
            {
                var groupToBeDeleted = groups.First();
                var groupRelatedCustomers = await _context.Customers.Where(g => g.GroupId == groupToBeDeleted.Id).ToListAsync();

                if(groupRelatedCustomers.Any())
                    return BadRequest("Grupa zawiera powiązanych kontrahentów.");

                groupToBeDeleted.IsArchived = true;

                try
                { 
                    _context.Groups.Update(groupToBeDeleted);
                    await _context.SaveChangesAsync();
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
