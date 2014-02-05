using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.Domain
{
    public class SwarmCoordinator
    {
        private readonly IWorkerFactory workerFactory;
        private readonly IProxyFactory proxyFactory;
        private readonly int maxWorkerCount;

        private readonly ProxyBag proxyBag;

        public SwarmCoordinator(IWorkerFactory workerFactory, IProxyFactory proxyFactory, int maxWorkerCount)
        {
            this.workerFactory = workerFactory;
            this.proxyFactory = proxyFactory;
            this.maxWorkerCount = maxWorkerCount;

            this.proxyBag = new ProxyBag();
        }
    }
}
