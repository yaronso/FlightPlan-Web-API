using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {
        private List<FlightPlan> FlightPlans = new List<FlightPlan>();

        // GET api/FlightPlan/5
        [HttpGet("{FlightID}", Name = "GetFlightID")]
        public ActionResult<FlightPlan> GetServerID(string id)
        {
            bool isOk = false;
            // iterate through the flights plan list
            foreach (var FlightPlan in FlightPlans)
            {
                if (FlightPlan.FlightPlanID.Equals(id))
                {
                    isOk = true;
                }
            }
            if (!isOk)
            {
                return NotFound();
            }
            return Ok(id);
        }

        // POST api/FlightPlan
        [HttpPost]
        public ActionResult Post(FlightPlan fp)
        {
            FlightPlans.Add(fp);
            return CreatedAtAction(actionName: "GetFlightID", new
            { FlightId = fp.FlightPlanID}, fp);
        }

    }
}