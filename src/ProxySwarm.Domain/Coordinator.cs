using ProxySwarm.Domain.Isolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.Domain
{
    public class Coordinator : IDisposable
    {
        private readonly Isolated<IsolatedSwarmCoordinator> isolatedCoordinator;

        private IsolatedSwarmCoordinator CoordinatorValue
        {
            get
            {
                return this.isolatedCoordinator.Value;
            }
        }

        public Coordinator(CoordinatorCreateOptions options)
        {
            this.isolatedCoordinator = new Isolated<IsolatedSwarmCoordinator>();
            this.CoordinatorValue.Initialize(options);
        }

        public void Start()
        {
            this.CoordinatorValue.Start();
        }

        public void Pause()
        {
            this.CoordinatorValue.Pause();
        }

        public void ReadProxiesFromFiles(string[] fileNames)
        {
            this.CoordinatorValue.ReadProxiesFromFiles(fileNames);
        }

        public Task<CounterChangeInfo> GetCounterChangeAsync()
        {
            var callback = new MarshaledResultSetter<CounterChangeInfo>();
            this.CoordinatorValue.NotifyOnCounterChange(callback);
            return callback.Task;
        }

        #region IDisposable
        public void Dispose()
        {
            if (this.isolatedCoordinator != null)
            {
                this.isolatedCoordinator.Dispose();
            }
        }
        #endregion //IDisposable
    }
}
