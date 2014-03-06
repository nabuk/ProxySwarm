using ProxySwarm.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.Domain.Logic
{
    internal interface IProxyFactory
    {
        IList<Proxy> RetrieveProxies(string content);
    }
}
