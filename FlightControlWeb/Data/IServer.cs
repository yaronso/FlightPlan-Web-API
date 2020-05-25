using FlightControlWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Data
{
    // The following interface sets the functionallity of the Servers objects.
    public interface IServer
    {
        IEnumerable<Servers> GetAllServers();
        Servers GetServerById(string id);
        void AddServer(Servers s);
        void DeleteServer(string id);
    }
}
