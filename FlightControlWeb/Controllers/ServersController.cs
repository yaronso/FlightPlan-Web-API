using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {
        private List<Servers> ServersList = new List<Servers>();
        
        // GET: api/servers
        [HttpGet]
        public ActionResult<List<Servers>> GetAllServers()
        {
            return ServersList;
        }

        // GET api/servers/5
        [HttpGet("{ServerId}" , Name = "GetServerID")]
        public ActionResult<Servers> GetServerID(string id)
        {
            bool isOk = false;
            // iterate through the servers list
            foreach (var Servers in ServersList) { 
                if(Servers.ServerId.Equals(id)) { 
                    isOk = true;
                }
            }
            if(!isOk) 
            {
                return NotFound();
            }
            return Ok(id);

        }

        /* GET api/servers/6
        [HttpGet("{ServerURL}", Name = "GetServerURL")]
        public ActionResult<Servers> GetServerURL(string URL)
        {
            bool isOk = false;
            // iterate through the servers list
            foreach (var Servers in ServersList)
            {
                if (Servers.ServerURL.Equals(URL))
                {
                    isOk = true;
                }
            }
            if (!isOk)
            {
                return NotFound();
            }
            return Ok(URL);

        }*/

        // POST api/servers
        [HttpPost]
        public ActionResult Post(Servers server)
        {
            ServersList.Add(server);
            return CreatedAtAction(actionName: "GetServerID", new
            {
                ServerId = server.ServerId,
                //ServerURL
            //= server.ServerURL
            },
                server);
        }

        // DELETE api/servers/7
        [HttpDelete("{ServerId}")]
        public string Delete(string id)
        {
            Servers s = ServersList.Where(x => x.ServerId == id).Single<Servers>();
            ServersList.Remove(s);
            return "Record has successfully Deleted";
        }

    }
}