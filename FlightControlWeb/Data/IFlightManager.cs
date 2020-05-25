using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Data
{
    // The following interface sets the functionallity of the Flight Manager objects.
    public interface IFlightManager
    {
        ConcurrentDictionary<string, Flight> getDic();
        IEnumerable<Flight> GetAllFlights();
        Flight GetFlightById(string id);
        void AddFlight(Flight f);
        void DeleteFlight(string id);
        Task<List<Flight>> get_All_FlightsAsync(string relative_to);
        Task<List<Flight>> get_Not_All_Async(string relative_to);
    }
}
