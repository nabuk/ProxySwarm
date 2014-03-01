using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.Domain
{
    [Serializable]
    public class CoordinatorCreateOptions
    {
        public int MaxWorkerCount { get; set; }

        //where TWorkerFactory : IProxyWorkerFactory, new()
        public Type ProxyWorkerFactoryType { get; set; }
    }
}
