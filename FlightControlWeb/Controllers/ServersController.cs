﻿using System;
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
        // Fields.
        public IServer managerServers;
        private static Random random = new Random();

        // The controller's constructor which has dependency injection of IServer object.
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
        public StatusCodeResult Post([FromBody] Servers s)
        {
            try
            {
                s.ServerId = RandomId();
                managerServers.AddServer(s);
                return StatusCode(StatusCodes.Status200OK);
            } catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        // DELETE: api/servers/5
        [HttpDelete("{id}")]
        public StatusCodeResult DeleteSer(string id)
        {
            try
            {
                managerServers.DeleteServer(id);
                return StatusCode(StatusCodes.Status200OK);
            } catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        // The following function randomize an id for each posted external server.
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