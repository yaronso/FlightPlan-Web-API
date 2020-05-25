using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    // The Flight class model which own the flights properties.
    public class Flight
    {   
        [Key]
        public string flight_id { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public int passengers { get; set; }
        public string company_name { get; set; }
        public string initial_location { get; set; }
        public string final_location { get; set; }
        public DateTime date_time { get; set; }
        public DateTime starting_time { get; set; }
        public string starting_datails { get; set; }
        public DateTime landing_time { get; set; }
        public string landing_details { get; set; }
        public bool is_external { get; set; }    
    }
}
