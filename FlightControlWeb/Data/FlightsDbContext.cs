using FlightControlWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Data
{
    public class FlightsDbContext : DbContext
    {
       public FlightsDbContext(DbContextOptions<FlightsDbContext>options):base(options)
        {

        }

        public DbSet<Flight> Flights { get; set; }

    }
}
