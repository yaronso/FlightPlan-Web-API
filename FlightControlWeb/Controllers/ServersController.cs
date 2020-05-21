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
        //public IServer manager = new ServerManager();
        public IServer managerServers;
        private static Random random = new Random();

        public ServersController(IServer manager)
        {
            this.managerServers = manager;
        }

        // GET: api/servers
        [HttpGet]
        public IEnumerable<Servers> GetServers()
        {
            return managerServers.GetAllServers();
        }

        // POST: api/servers/6
        [HttpPost]
        public void Post([FromBody] Servers s)
        {
            s.ServerId = RandomId();
            managerServers.AddServer(s);
        }

        // DELETE: api/servers/5
        [HttpDelete("{id}")]
        public void DeleteSer(string id)
        {
            managerServers.DeleteServer(id);
        }

        private string RandomId()
        {            
            const string charsNum = "0123456789";
            string nums;           

            nums = new string(Enumerable.Repeat(charsNum, 6)
             .Select(s => s[random.Next(s.Length)]).ToArray());

            return nums;
        }
    }
}