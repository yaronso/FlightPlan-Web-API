using FlightControlWeb.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Data
{
    interface IFlightManager
    {
        ConcurrentDictionary<string, Flight> getDic();
        IEnumerable<Flight> GetAllFlights();
        Flight GetFlightById(string id);
        void AddFlight(Flight f);
        void DeleteFlight(string id);
    }
}
