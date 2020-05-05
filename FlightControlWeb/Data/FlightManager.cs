using FlightControlWeb.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Data
{
    public class FlightManager : IFlightManager
    {
        // The Flights dictionary
        public static ConcurrentDictionary<string, Flight> FlightsDic
        = new ConcurrentDictionary<string, Flight>();

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
    }
}
