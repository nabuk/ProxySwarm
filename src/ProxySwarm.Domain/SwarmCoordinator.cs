using ProxySwarm.Domain.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySwarm.Domain
{
    public class SwarmCoordinator : IDisposable
    {
        private readonly IProxyWorkerFactory proxyWorkerFactory;
        private readonly int maxWorkerCount;

        private readonly ProxyBag proxyBag;

        public SwarmCoordinator(IProxyWorkerFactory proxyWorkerFactory, IObservable<Proxy> proxySource, int maxWorkerCount)
        {
            this.proxyWorkerFactory = proxyWorkerFactory;
            this.maxWorkerCount = maxWorkerCount;
            this.proxyBag = new ProxyBag(proxySource);
            this.Status = new SwarmCoordinatorStatus(this.proxyBag.Counter);
        }

        public void Start()
        {
            
        }

        public void Pause()
        {
            
        }

        public SwarmCoordinatorStatus Status { get; private set; }

        #region IDisposable
        public void Dispose()
        {
            this.proxyBag.Dispose();
        }
        #endregion //IDisposable
    }
}
