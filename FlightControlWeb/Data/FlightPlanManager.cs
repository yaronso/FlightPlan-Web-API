using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Data
{
    // A conrete class which implements the IFlightPlanManger interface for dependency
    // injection in the FlightPlancontroller.
    public class FlightPlanManager : IFlightPlanManager
    {
        // Init the Flight Plans dictionary.
        public static ConcurrentDictionary<string, FlightPlan> FlightPlansDic
        = new ConcurrentDictionary<string, FlightPlan>();
        // Adding a flight plan to the dictionary.
        public void AddFlightPlan(FlightPlan fp)
        {
            FlightPlansDic.TryAdd(fp.flightPlanID, fp);
        }

        // Delete a flight plan from the dictionary.
        public void DeleteFlightPlan(string id)
        {
            FlightPlan delete = GetFlightPlanById(id);
            FlightPlansDic.TryRemove(id, out delete);
        }

        // Get the whole list of flight plans.
        public IEnumerable<FlightPlan> GetAllFlightPlans()
        {
            List<FlightPlan> flightPlans = FlightPlansDic.Values.ToList();
            return flightPlans;
        }

        // Get a specific flight plan by id.
        public FlightPlan GetFlightPlanById(string id)
        {
            FlightPlansDic.TryGetValue(id, out FlightPlan value);
            // Verify that the flight plan is not null.
            if(value != null)
            {
                return value;
            }
            return null;
        }

 
    }
}
