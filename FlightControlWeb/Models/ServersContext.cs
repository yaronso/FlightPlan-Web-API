using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class ServersContext : DbContext
    {
        public ServersContext(DbContextOptions<ServersContext> options)
         : base(options)
        {
        }
        // The Servers DB
        public DbSet<Servers> ServersDB { get; set; }
    }
}
