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
        private readonly IWorkerFactory workerFactory;
        private readonly int maxWorkerCount;

        private readonly ProxyBag proxyBag;

        public SwarmCoordinator(IWorkerFactory workerFactory, int maxWorkerCount)
        {
            this.workerFactory = workerFactory;
            this.maxWorkerCount = maxWorkerCount;
            this.proxyBag = new ProxyBag();
            this.Status = new SwarmCoordinatorStatus();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public SwarmCoordinatorStatus Status { get; private set; }

        #region IDisposable
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion //IDisposable
    }
}
