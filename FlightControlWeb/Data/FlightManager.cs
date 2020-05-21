using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlightControlWeb.Data
{
    public class FlightManager : IFlightManager
    {
        // The Flights dictionary
        public static ConcurrentDictionary<string, Flight> FlightsDic
        = new ConcurrentDictionary<string, Flight>();
        string format = "yyyy-MM-dd'T'HH:mm:ss'Z'";
        CultureInfo provider = CultureInfo.InvariantCulture;


        // The flights Dic getter
        public ConcurrentDictionary<string, Flight> getDic()
        {
            return FlightsDic;
        }

        public void AddFlight(Flight f)
        {
            FlightsDic.TryAdd(f.flight_id, f);
        }

        public void DeleteFlight(string id)
        {
            Flight delete = GetFlightById(id);
            FlightsDic.TryRemove(id, out delete);
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            List<Flight> Flights = FlightsDic.Values.ToList();
            return Flights;
        }

        public Flight GetFlightById(string id)
        {
            FlightsDic.TryGetValue(id, out Flight value);
            if (value != null)
            {
                return value;
            }
            return null;
        }

        // Triggered when we dont have sync all parameter.
        public async Task<List<Flight>> get_Not_All_Async(string relative_to)
        {
            ConcurrentDictionary<string, Flight> FlightsDic = getDic();
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

        // Triggered when we recieve sync all parameter.
        public async Task<List<Flight>> get_All_FlightsAsync(string relative_to)
        {
            ConcurrentDictionary<string, Flight> FlightsDic = getDic();
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
            
            return list;
        }

        private async Task getFlightPlansFromExServerAsync(List<Flight> exFlights, string ur)
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
                FlightPlanManager.FlightPlansDic.TryAdd(fp.flightPlanID, fp);

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
        }

    }
}

