using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Servers
    {
        public string ServerId { get; set; }
        public string ServerURL { get; set; }
        //  Each server will have a list of his flight plans.
        public List<FlightPlan> FlightPlans { get; set; }
    }
}
