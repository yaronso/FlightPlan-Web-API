using FlightControlWeb.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightControlWeb.Data
{
    public class ServerManager : IServer
    {
        // The servers dictionary
        private static ConcurrentDictionary<string, Servers> ServersDic
        = new ConcurrentDictionary<string, Servers>();
        public void AddServer(Servers s)
        {
            ServersDic.TryAdd(s.ServerId, s);
        }

        public void DeleteServer(string id)
        {
            Servers delete = GetServerById(id);
            ServersDic.TryRemove(id, out delete);
        }

        public IEnumerable<Servers> GetAllServers()
        {
            List<Servers> Servers = ServersDic.Values.ToList();
            return Servers;
        }

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
