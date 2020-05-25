using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Data
{
    // The following interface sets the functionallity of the FlightPlan Manager objects.
    public interface IFlightPlanManager
    {
        IEnumerable<FlightPlan> GetAllFlightPlans();
        FlightPlan GetFlightPlanById(string id);
        void AddFlightPlan(FlightPlan p);
        void DeleteFlightPlan(string id);
    }
}
