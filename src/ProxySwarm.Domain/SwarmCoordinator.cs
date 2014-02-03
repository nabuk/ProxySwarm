using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.Domain
{
    public class SwarmCoordinator
    {
        private readonly ProxyBag proxyBag;
        private readonly IWorkerFactory workerFactory;
        private readonly int maxWorkerCount;

        public SwarmCoordinator(ProxyBag proxyBag, IWorkerFactory workerFactory, int maxWorkerCount)
        {
            this.proxyBag = proxyBag;
            this.workerFactory = workerFactory;
            this.maxWorkerCount = maxWorkerCount;
        }


    }
}
