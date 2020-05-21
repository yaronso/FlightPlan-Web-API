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
        //IFlightPlanManager managerFlightPlan = new FlightPlanManager();
        IFlightPlanManager managerFlightPlan;
        private static Random random = new Random();

        public FlightPlanController(IFlightPlanManager managerFP)
        {
            this.managerFlightPlan = managerFP;
        }

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
            FlightPlan fp = managerFlightPlan.GetFlightPlanById(id);
            // Check if the flight plan is null.
            if(fp == null)
            {
                throw new ArgumentException($"There is no Flight Plan with the id {id}", nameof(id));
            }
            return managerFlightPlan.GetFlightPlanById(id);
        }
        // POST: api/FlightPlan/6
        [HttpPost]
        public void Post(FlightPlan fp)
        {
            string id = this.RandomString();
            fp.flightPlanID = id;
            Flight flight = new Flight();
            flight.flight_id = id;
            flight.passengers = fp.passengers;
            flight.company_name = fp.company_name;
            ////flight.starting_time = fp.initial_location.date_time;
            flight.starting_datails = fp.initial_location.date_time.ToString("dd-MM-yyyy");
            flight.initial_location = "Lat: " + fp.initial_location.latitude.ToString() + "  Lon: " + fp
                .initial_location.longitude.ToString();
            flight.date_time = fp.initial_location.date_time;
            flight.longitude = fp.initial_location.longitude;
            flight.latitude = fp.initial_location.latitude;
            flight.is_external = false;
            double end = 0;

            int i;
            for (i = 0; i < fp.segments.Count; i++)
            {
                end += fp.segments[i].timespan_seconds;
            }

            flight.final_location = "Lat: " + fp.segments[i-1].latitude.ToString() + "  Lon: " + 
                fp.segments[i - 1].longitude.ToString();
            DateTime landing = new DateTime();
            landing = fp.initial_location.date_time.AddSeconds(end);            
            flight.landing_time = landing;
            flight.landing_details = landing.ToString("dd-MM-yyyy");
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