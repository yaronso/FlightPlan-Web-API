using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Data;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        IServer manager = new ServerManager();

        // GET: api/servers
        [HttpGet]
        public IEnumerable<Servers> GetServers()
        {
            return manager.GetAllServers();
        }

        // POST: api/servers/6
        [HttpPost]
        public void Post([FromBody] Servers s)
        {
            manager.AddServer(s);
        }

        // DELETE: api/servers/5
        [HttpDelete("{id}")]
        public void DeleteSer(string id)
        {
           manager.DeleteServer(id);
        }
    }
}