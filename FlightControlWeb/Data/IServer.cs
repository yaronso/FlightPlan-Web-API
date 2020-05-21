using FlightControlWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Data
{
    public interface IServer
    {
        IEnumerable<Servers> GetAllServers();
        Servers GetServerById(string id);
        void AddServer(Servers s);
        void DeleteServer(string id);
    }
}
