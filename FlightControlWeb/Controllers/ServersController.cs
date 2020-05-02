using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightControlWeb.Models;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private readonly ServersContext _context;

        public ServersController(ServersContext context)
        {
            _context = context;
        }

        // GET: api/Servers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Servers>>> GetServersDB()
        {
            return await _context.ServersDB.ToListAsync();
        }

        // GET: api/Servers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Servers>> GetServers(string id)
        {
            var servers = await _context.ServersDB.FindAsync(id);

            if (servers == null)
            {
                return NotFound();
            }

            return servers;
        }

        // PUT: api/Servers/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServers(string id, Servers servers)
        {
            if (id != servers.ServerId)
            {
                return BadRequest();
            }

            _context.Entry(servers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Servers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Servers>> PostServers(Servers servers)
        {
            _context.ServersDB.Add(servers);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ServersExists(servers.ServerId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetServers", new { id = servers.ServerId }, servers);
        }

        // DELETE: api/Servers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Servers>> DeleteServers(string id)
        {
            var servers = await _context.ServersDB.FindAsync(id);
            if (servers == null)
            {
                return NotFound();
            }

            _context.ServersDB.Remove(servers);
            await _context.SaveChangesAsync();

            return servers;
        }

        private bool ServersExists(string id)
        {
            return _context.ServersDB.Any(e => e.ServerId == id);
        }
    }
}
