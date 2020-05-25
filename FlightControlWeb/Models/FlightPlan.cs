using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FlightControlWeb.Models
{
    // Inner class inside FligtPlan class.
    [Owned]
    public class InitialLocation 
    {
           public double longitude { get; set; }
           public double latitude { get; set; }
           public DateTime date_time { get; set; }
    }
    // Inner class inside FlightPlan class.
    [Owned]
    public class Segment
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
        public double timespan_seconds { get; set; }
     }

    // The FlightPlan class model which own the FP properties with sub classes.
    public class FlightPlan
    {
        [Key]
        public string flightPlanID { get; set; }
        public int passengers { get; set; }
        public string company_name { get; set; }
        [Key]
        public InitialLocation initial_location { get; set; }
        [Key]
        public IList<Segment> segments { get; set; }   
    }   
    
}
