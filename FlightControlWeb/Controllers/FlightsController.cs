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
        // The flights manager.
        IFlightManager managerFlight = new FlightManager();
        string format = "yyyy-MM-dd'T'HH:mm:ss'Z'";
        CultureInfo provider = CultureInfo.InvariantCulture;

        // GET: api/Flights
        /*
        [HttpGet]        
        public IEnumerable<Flight> GetAllFlight()
        {
            return managerFlight.GetAllFlights();
        }*/



        //[HttpGet("{date}", Name = "Get")]

        
    [HttpGet]
    public  IEnumerable<Flight> Get()
    {
       string relative_to = Request.Query["relative_to"].ToString();
        // get the flight dictionary
        ConcurrentDictionary<string, Flight> FlightsDic = managerFlight.getDic();
       DateTime dateTime = ParseDateTime(relative_to);
       List<Flight> list = new List<Flight>();
       foreach (var keyValuePair in FlightsDic)
       {
            if (keyValuePair.Value.date_time <= dateTime && 
                keyValuePair.Value.landing_time >= dateTime &&
                keyValuePair.Value.is_external == false)
            {
                Interpolation(keyValuePair.Value, dateTime);
                list.Add(keyValuePair.Value);
            }
       }
       return list;
    }

        [HttpGet("{sync_all?}")]
        public async Task<List<Flight>> Get([FromQuery] string relative_to, [FromQuery]string? sync_all)
        {
            //string relative_to = Request.Query["relative_to"].ToString();
            // get the flight dictionary
            ConcurrentDictionary<string, Flight> FlightsDic = managerFlight.getDic();
            DateTime dateTime = ParseDateTime(relative_to);
            List<Flight> list = new List<Flight>();
            foreach (var keyValuePair in FlightsDic)
            {
                if (keyValuePair.Value.date_time <= dateTime &&
                    keyValuePair.Value.landing_time >= dateTime &&
                    keyValuePair.Value.is_external == false)
                {
                    Interpolation(keyValuePair.Value, dateTime);
                    list.Add(keyValuePair.Value);
                }
            }

            if (Request.QueryString.Value.Contains("sync_all"))
            {
                string url;                
                List<Flight> exFlights = new List<Flight>();
                foreach (var keyValuePair in ServerManager.ServersDic)
                {
                    url = keyValuePair.Value.ServerURL;
                    url = url + "/api/Flights?relative_to=" + DateTime.Now.ToString(format);
                    using (var client = new HttpClient())
                    {
                        var content = await client.GetStringAsync(url);
                        exFlights = JsonConvert.DeserializeObject<List<Flight>>(content);
                        await getFlightPlansFromExServerAsync(exFlights, keyValuePair.Value.ServerURL);
                        list.AddRange(exFlights);

                    }

                }

                
            }

            return list;
        }

        private async Task getFlightPlansFromExServerAsync(List<Flight> exFlights , string ur)
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
            
        }        
    }
}
