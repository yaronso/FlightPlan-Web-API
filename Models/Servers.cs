﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    
    public class Servers
    {
        [Key]
        public string ServerId { get; set; }
        public string ServerURL { get; set; }
    }
}
