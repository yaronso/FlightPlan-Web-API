using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FlightControlWeb.Data;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        // Field.
        IFlightManager managerFlight;

        // The controller's constructor which has dependency injection of IFlightManager object.
        public FlightsController(IFlightManager managerFlight)
        {
            this.managerFlight = managerFlight;
        } 

        [HttpGet("{sync_all?}")]
       // public async Task<IActionResult> Get([FromQuery] string relative_to, [FromQuery] string sync_all)
        public async Task<List<Flight>> Get([FromQuery] string relative_to, [FromQuery] string sync_all)
        {
            // If we got the sync all parameter.
            if (Request.QueryString.Value.Contains("sync_all"))
            {
                    //return Ok(await managerFlight.get_All_FlightsAsync(relative_to));
                    return await managerFlight.get_All_FlightsAsync(relative_to);
            }
            // if we dont have the sync all parameter.
            return await managerFlight.get_Not_All_Async(relative_to);

        }

        // POST: api/Flights
        [HttpPost]
        public void Post([FromBody] Flight f)
        {
            managerFlight.AddFlight(f);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public StatusCodeResult DeleteFlight(string id)
        {
            try
            {
                managerFlight.DeleteFlight(id);
                return StatusCode(StatusCodes.Status200OK);
            } catch
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

        }      
    }
}
