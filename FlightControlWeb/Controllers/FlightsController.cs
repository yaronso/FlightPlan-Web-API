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
        //IFlightManager managerFlight = new FlightManager();
        IFlightManager managerFlight;
        //string format = "yyyy-MM-dd'T'HH:mm:ss'Z'";
        //CultureInfo provider = CultureInfo.InvariantCulture;

        public FlightsController(IFlightManager managerFlight)
        {
            this.managerFlight = managerFlight;
        } 

        [HttpGet("{sync_all?}")]
        public async Task<List<Flight>> Get([FromQuery] string relative_to, [FromQuery] string sync_all)
        {
            // if we got sync all parameter.
            if(Request.QueryString.Value.Contains("sync_all"))
            {
               return await managerFlight.get_All_FlightsAsync(relative_to);
            }
            // if we dont have sync all parameter.
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
        public void DeleteFlight(string id)
        {
            managerFlight.DeleteFlight(id);
        }

        /*private async Task getFlightPlansFromExServerAsync(List<Flight> exFlights , string ur)
        {
            string url = ur + "/api/FlightPlan/";
            FlightPlan fp = new FlightPlan();
            var client = new HttpClient();                        
            foreach (var flight in exFlights)
            {
                flight.is_external = true;
                url = url + flight.flight_id;
                var content = await client.GetStringAsync(url);
                fp = JsonConvert.DeserializeObject<FlightPlan>(content);
                fp.flightPlanID = flight.flight_id;
                FlightPlanManager.FlightPlansDic.TryAdd(fp.flightPlanID,fp);               

            }

        }


        private DateTime ParseDateTime(string d)
        {
            DateTime res;
            res = DateTime.ParseExact(d, this.format, this.provider);
            Debug.WriteLine(res.ToString());
            return res;
        }

        private void Interpolation(Flight flight, DateTime date)
        {
            int i = 0;
            DateTime init_date = FlightPlanManager.FlightPlansDic[flight.flight_id].
                initial_location.date_time;
         
            IList<Segment> segments = FlightPlanManager.FlightPlansDic[flight.flight_id].segments;
            DateTime new_date_seg = init_date.AddSeconds(segments[i].timespan_seconds);

            while (!(new_date_seg > date && date >= init_date))
            {
                i++;
                init_date = new_date_seg;
                DateTime refresh_Seg = new_date_seg.AddSeconds(segments[i].timespan_seconds);
                new_date_seg = refresh_Seg;
            }

            // starting point (condition i = 0) the previus location
            double start_lon, start_lat, end_lon, end_lat;

            if (i == 0)
            {
                start_lon = FlightPlanManager.FlightPlansDic[flight.flight_id].
                 initial_location.longitude;

                start_lat = FlightPlanManager.FlightPlansDic[flight.flight_id].
                initial_location.latitude;

                double diffInSeconds = (date - init_date).TotalSeconds;
                double prop = diffInSeconds / segments[i].timespan_seconds;
                end_lon = start_lon + (segments[i].longitude - start_lon) * prop;
                end_lat = start_lat + (segments[i].latitude - start_lat) * prop;

            }
            else
            {

                start_lon = segments[i - 1].longitude;
                start_lat = segments[i - 1].latitude;

                double end = 0;

                for (int j = 0; j < i; j++)
                {
                    end += segments[j].timespan_seconds;
                }

                DateTime init = FlightPlanManager.FlightPlansDic[flight.flight_id].
                initial_location.date_time.AddSeconds(end);

                double diffInSeconds = (date - init).TotalSeconds;

                double prop = diffInSeconds / segments[i].timespan_seconds;
                end_lon = start_lon + (segments[i].longitude - start_lon) * prop;
                end_lat = start_lat + (segments[i].latitude - start_lat) * prop;

            }

            flight.longitude = end_lon;
            flight.latitude = end_lat;               
        }*/        
    }
}
