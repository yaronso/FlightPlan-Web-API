using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Data;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        IFlightPlanManager manager = new FlightPlanManager();

        // GET: api/FlightPlan
        [HttpGet]
        public IEnumerable<FlightPlan> GetAllFlightPLans()
        {
            return manager.GetAllFlightPlans();
        }

        // GET: api/FlightPlan/5
        [HttpGet("{id}")]
        public FlightPlan GetFlightPlan(string id)
        {
            return manager.GetFlightPlanById(id);

        }
        // POST: api/FlightPlan/6
        [HttpPost]
        public void Post([FromBody] FlightPlan fp)
        {
           manager.AddFlightPlan(fp);
        }

    }
}