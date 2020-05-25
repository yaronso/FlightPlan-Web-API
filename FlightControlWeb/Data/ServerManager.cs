using FlightControlWeb.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Data
{
    // A conrete class which implements the IServer interface for dependency
    // injection in the Serverscontroller. 
    // It represents the external servers which the main server sync with. 
    public class ServerManager : IServer
    {
        // Init the servers dictionary.
        public static ConcurrentDictionary<string, Servers> ServersDic
        = new ConcurrentDictionary<string, Servers>();

        // Add a new server for the dictionary.
        public void AddServer(Servers s)
        {
            ServersDic.TryAdd(s.ServerId, s);
        }

        // Delete a server from the dictionary.
        public void DeleteServer(string id)
        {
            Servers delete = GetServerById(id);
            ServersDic.TryRemove(id, out delete);
        }

        // Get the whole set of servers from the dictionary.
        public IEnumerable<Servers> GetAllServers()
        {
            List<Servers> Servers = ServersDic.Values.ToList();
            return Servers;
        }

        // Get a specific server by his ID.
        public Servers GetServerById(string id)
        {
            ServersDic.TryGetValue(id, out Servers value);
            if (value != null)
            {
                return value;
            }
            return null;
        }
    }
}
