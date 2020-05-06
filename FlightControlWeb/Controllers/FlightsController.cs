using System;
using System.Collections.Concurrent;
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
        // The flights manager.
        IFlightManager managerFlight = new FlightManager();
        string format = "yyyy-MM-dd'T'HH:mm:ss'Z'";
        CultureInfo provider = CultureInfo.InvariantCulture;

        // GET: api/Flights
        [HttpGet]        
        public IEnumerable<Flight> GetAllFlight()
        {
            return managerFlight.GetAllFlights();
        }

        [HttpGet("{date}", Name = "Get")]
        public IEnumerable<Flight> Get(string date)
        {
           // get the flight dictionary
           ConcurrentDictionary<string, Flight> FlightsDic = managerFlight.getDic();
           DateTime dateTime = ParseDateTime(date);
           List<Flight> list = new List<Flight>();
           foreach (var keyValuePair in FlightsDic)
           {
                if (keyValuePair.Value.date_time <= dateTime && 
                    keyValuePair.Value.landing_time >= dateTime &&
                    keyValuePair.Value.is_external == false)
                {
                    list.Add(keyValuePair.Value);
                }
           }
           return list;
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
        public void Post([FromBody] Flight f)
        {
            managerFlight.AddFlight(f);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteFlight(string id)
        {
            managerFlight.DeleteFlight(id);
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
