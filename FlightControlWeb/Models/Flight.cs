﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Flight
    {
        public string flight_id { get; set; }
        public double longtitude { get; set; }
        public double latitude { get; set; }
        public int passengers { get; set; }
        public string company_name { get; set; }
        public DateTime date_time { get; set; }
        public bool is_external { get; set; }
    }
}
