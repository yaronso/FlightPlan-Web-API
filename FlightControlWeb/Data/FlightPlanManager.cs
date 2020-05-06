using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Data
{
    public class FlightPlanManager : IFlightPlanManager
    {
        public static ConcurrentDictionary<string, FlightPlan> FlightPlansDic
       = new ConcurrentDictionary<string, FlightPlan>();

        public void AddFlightPlan(FlightPlan fp)
        {
            FlightPlansDic.TryAdd(fp.flightPlanID, fp);
        }

        public void DeleteFlightPlan(string id)
        {
            FlightPlan delete = GetFlightPlanById(id);
            FlightPlansDic.TryRemove(id, out delete);
        }

        public IEnumerable<FlightPlan> GetAllFlightPlans()
        {
            List<FlightPlan> flightPlans = FlightPlansDic.Values.ToList();
            return flightPlans;
        }

        public FlightPlan GetFlightPlanById(string id)
        {
            FlightPlansDic.TryGetValue(id, out FlightPlan value);
            if(value != null)
            {
                return value;
            }
            return null;
        }

 
    }
}
