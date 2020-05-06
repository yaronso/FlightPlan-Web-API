using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class FlightPlanController : ControllerBase
    {
        IFlightPlanManager managerFlightPlan = new FlightPlanManager();
        private static Random random = new Random();

        // GET: api/FlightPlan
        [HttpGet]
        public IEnumerable<FlightPlan> GetAllFlightPLans()
        {
            return managerFlightPlan.GetAllFlightPlans();
        }

        // GET: api/FlightPlan/5
        [HttpGet("{id}")]
        public FlightPlan GetFlightPlan(string id)
        {
            return managerFlightPlan.GetFlightPlanById(id);

        }
        // POST: api/FlightPlan/6
        [HttpPost]
        public void Post([FromBody] FlightPlan fp)
        {
            string id = this.RandomString();
            fp.flightPlanID = id;
            Flight flight = new Flight();
            flight.flight_id = id;
            flight.passengers = fp.passengers;
            flight.company_name = fp.company_name;
            flight.date_time = fp.initial_location.date_time;
            flight.longtitude = fp.initial_location.longitude;
            flight.latitude = fp.initial_location.latitude;
            flight.is_external = false;
            double end = 0;
          
            for (int i = 0; i < fp.segments.Count; i++)
            {
                end += fp.segments[i].timespan_seconds;
            }

            DateTime landing = new DateTime();
            landing = fp.initial_location.date_time.AddSeconds(end);            
            flight.landing_time = landing;
            FlightManager.FlightsDic.TryAdd(flight.flight_id, flight);
            managerFlightPlan.AddFlightPlan(fp);
            
        }

        private string RandomString()
        {
            const string charsLet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string charsNum = "0123456789";
            string nums, letters, res;
            letters = new string(Enumerable.Repeat(charsLet, 3)
              .Select(s => s[random.Next(s.Length)]).ToArray());

           nums = new string(Enumerable.Repeat(charsNum, 3)
            .Select(s => s[random.Next(s.Length)]).ToArray());

            res = letters + nums;
            return res;
        }

    }
}