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
    // A conrete class which implements the IFlightManger interface for dependency
    // injection in the Flightscontroller.
    public class FlightManager : IFlightManager
    {
        // Init the Flights dictionary.
        public static ConcurrentDictionary<string, Flight> FlightsDic
        = new ConcurrentDictionary<string, Flight>();
        string format = "yyyy-MM-dd'T'HH:mm:ss'Z'";
        CultureInfo provider = CultureInfo.InvariantCulture;


        // Getter for the Flights dictionary.
        public ConcurrentDictionary<string, Flight> getDic()
        {
            return FlightsDic;
        }

        // Adding a flight to the dictionary.
        public void AddFlight(Flight f)
        {
            FlightsDic.TryAdd(f.flight_id, f);
        }

        // Delete a flight from the dictionary.
        public void DeleteFlight(string id)
        {
            Flight delete = GetFlightById(id);
            FlightsDic.TryRemove(id, out delete);
        }

        // Get the whole flights from the dictionary.
        public IEnumerable<Flight> GetAllFlights()
        {
            List<Flight> Flights = FlightsDic.Values.ToList();
            return Flights;
        }

        // Get a specific flight by flight id.
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
            // Iterate through the pairs in the Flights dictionary.
            foreach (var keyValuePair in FlightsDic)
            {
                // verify that the date time is valid.
                if (keyValuePair.Value.date_time <= dateTime &&
                    keyValuePair.Value.landing_time >= dateTime &&
                    keyValuePair.Value.is_external == false)
                {
                    try
                    {
                        Interpolation(keyValuePair.Value, dateTime);
                    }
                    catch (Exception e)
                    {
                        e.ToString();
                    }
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
            //Iterate through the Flights Dictionary.
            foreach (var keyValuePair in FlightsDic)
            {
                // Verify that the date time is valid.
                if (keyValuePair.Value.date_time <= dateTime &&
                    keyValuePair.Value.landing_time >= dateTime &&
                    keyValuePair.Value.is_external == false)
                {
                    try
                    {
                        Interpolation(keyValuePair.Value, dateTime);
                    } catch (Exception e)
                    {
                        e.ToString();
                    }  
                    list.Add(keyValuePair.Value);
                }
            }
 
            string url;
            List<Flight> exFlights = new List<Flight>();
            // Iterate through the Servers dictionary and get the external flights from each sever.
            foreach (var keyValuePair in ServerManager.ServersDic)
            {
                try
                {
                    url = keyValuePair.Value.ServerURL;
                    url = url + "/api/Flights?relative_to=" + DateTime.Now.AddHours(-3).ToString(format);
                    // Create a http client request.
                    using (var client = new HttpClient())
                    {
                        var content = await client.GetStringAsync(url);
                        // Convert the data from json to C# object.
                        exFlights = JsonConvert.DeserializeObject<List<Flight>>(content);
                        await getFlightPlansFromExServerAsync(exFlights, keyValuePair.Value.ServerURL);
                        // Add the external flights to the main flights list.
                        list.AddRange(exFlights);
                    }
                } catch (Exception e)
                {
                    e.ToString();
                    Debug.Write("sync all catch");
                    //return list;
                    continue;
                }

            }

            Debug.Write("list size and content" + list.Count + "  " + list[0].company_name);
            return list;
        }

        // The following function is used for getting the correct Flight Plan according the 
        // flight id by a http client request. it will invoke the client side's function which
        // will draw the path of the flight plan.
        private async Task getFlightPlansFromExServerAsync(List<Flight> exFlights, string ur)
        {
            FlightPlan fp = new FlightPlan();
            var client = new HttpClient();
            foreach (var flight in exFlights)
            {
                string url = ur + "/api/FlightPlan/";
                flight.is_external = true;
                url = url + flight.flight_id;
                var content = await client.GetStringAsync(url);
                fp = JsonConvert.DeserializeObject<FlightPlan>(content);
                fp.flightPlanID = flight.flight_id;
                flight.starting_datails = fp.initial_location.date_time.ToString("dd-MM-yyyy");
                flight.initial_location = "Lat: " + fp.initial_location.latitude.ToString() + "  Lon: " + fp
                    .initial_location.longitude.ToString();
                double end = 0;

                int i;
                for (i = 0; i < fp.segments.Count; i++)
                {
                    // Count the total time in seconds of the whole segments.
                    end += fp.segments[i].timespan_seconds;
                }

                flight.final_location = "Lat: " + fp.segments[i - 1].latitude.ToString() + "  Lon: " +
                    fp.segments[i - 1].longitude.ToString();
                DateTime landing = new DateTime();
                landing = fp.initial_location.date_time.AddSeconds(end);
                flight.landing_time = landing;
                flight.landing_details = landing.ToString("dd-MM-yyyy");
                FlightPlanManager.FlightPlansDic.TryAdd(fp.flightPlanID, fp);
            }
        }

        // The following function parses the date time according the required date format.
        private DateTime ParseDateTime(string d)
        {
            DateTime res;
            res = DateTime.ParseExact(d, this.format, this.provider);
            return res;
        }

        // The following function computes the path of the airplane 
        // according his current time and his speed.
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

