using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
        public class InitialLocation
        {
            public double longitude { get; set; }
            public double latitude { get; set; }
            public DateTime date_time { get; set; }
        }

        // https://www.jsonutils.com/
        public class FlightPlan
        {
            public int passengers { get; set; }
            public string company_name { get; set; }
            public InitialLocation initial_location { get; set; }
            public IList<object> segments { get; set; }   // segments: lon, lat, time per sec, etc.
        }

    
}
