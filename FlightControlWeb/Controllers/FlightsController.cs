using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Data;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        FlightsDbContext flightDbContext;
        string format = "yyyy-MM-dd'T'HH:mm:ss'Z'";
        CultureInfo provider = CultureInfo.InvariantCulture;


        public FlightsController(FlightsDbContext _FlightsDbContext)
        {
            this.flightDbContext = _FlightsDbContext;
            
        }


        // GET: api/Flights
        [HttpGet]        
        public async Task<ActionResult<IEnumerable<Flight>>> Get()
        {
            //return flightDbContext.Flights.Where(m=>m.ParseDateTime(m.flight_id) > ParseDateTime(date));
            return await flightDbContext.Flights.ToListAsync();
        }

        [HttpGet("{date}", Name = "Get")]
        public IEnumerable<Flight> Get(string date)
        {
            return flightDbContext.Flights.Where
                (m => m.date_time < ParseDateTime(date));
        }

        /*
        // GET: api/Flights/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(string id)
        {
            var flight = flightDbContext.Flights.SingleOrDefault(m=>m.flight_id.Equals(id));
            if (flight == null)
            {
                return NotFound("No Record Found");
            }
            //ParseDateTime("2020-12-26T23:56:21Z");
            return Ok(flight);
        }*/

        // POST: api/Flights
        [HttpPost]
        public IActionResult Post([FromBody] Flight f)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            flightDbContext.Flights.Add(f);
            //flightDbContext.SaveChangesAsync
            flightDbContext.SaveChanges(true);
            return StatusCode(StatusCodes.Status201Created);
        } 

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var flight = flightDbContext.Flights.Find(id);
            if (flight == null)
            {
                return NotFound("No Record Found");
            }
            flightDbContext.Flights.Remove(flight);
            flightDbContext.SaveChanges(true);
            return Ok("Flight Deleted"); 
        }

        private DateTime ParseDateTime(string d)
        {
            DateTime res;
            res = DateTime.ParseExact(d, this.format, this.provider);
            Debug.WriteLine(res.ToString());
            return res;
        }

     
    }
}
