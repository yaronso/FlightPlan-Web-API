using FlightControlWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Data
{
    interface IFlightPlanManager
    {
        IEnumerable<FlightPlan> GetAllFlightPlans();
        FlightPlan GetFlightPlanById(string id);
        void AddFlightPlan(FlightPlan p);
        void DeleteFlightPlan(string id);
    }
}
