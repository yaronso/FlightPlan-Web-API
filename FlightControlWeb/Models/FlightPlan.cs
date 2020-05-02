﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FlightControlWeb.Models
{
    [Owned]
    public class InitialLocation 
      {
           public int Id { get; set; }   // <----- id for the DB context
           public double longitude { get; set; }
           public double latitude { get; set; }
           public DateTime date_time { get; set; }
    }

    [Owned]
    public class Segment
    {
        public int Id  { get; set ; }   // <----- id - for the DB context
        public double longitude { get; set; }
        public double latitude { get; set; }
        public int timespan_seconds { get; set; }
     }

    // https://www.jsonutils.com/
    public class FlightPlan
    {
        // TODO - verify about FlightPlanID
        
        public string FlightPlanID { get; set; }
        public int passengers { get; set; }
        public string company_name { get; set; }
        [Key]
        public InitialLocation initial_location { get; set; }
        [Key]
        public IList<Segment> seg { get; set; }   // segments: lon, lat, time per sec, etc.
    }

    
}
