using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.Domain.ProxyFactory
{
    public interface IProxyFactory
    {
        IList<Proxy> RetrieveProxies(string content);
    }
}
