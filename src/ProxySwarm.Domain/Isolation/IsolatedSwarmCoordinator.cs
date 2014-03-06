using ProxySwarm.Domain.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySwarm.Domain.Isolation
{
    public class IsolatedSwarmCoordinator : MarshalByRefObject, IDisposable
    {
        private SwarmCoordinator swarmCoordinator;
        private ProxyFileSource proxySource;
        private IsolatedCounter[] counters;

        public void Initialize(CoordinatorCreateOptions options)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            System.Net.ServicePointManager.DefaultConnectionLimit = options.MaxWorkerCount;

            this.proxySource = new ProxyFileSource(new DefaultProxyFactory());
            var proxyWorkerFactory = (IProxyWorkerFactory)Activator.CreateInstance(options.ProxyWorkerFactoryType);
            this.swarmCoordinator = new SwarmCoordinator(proxyWorkerFactory, this.proxySource, options.MaxWorkerCount);
            this.counters = new []
            { 
                new IsolatedCounter(CounterChangeInfo.SuccessesKey, this.swarmCoordinator.Status.SuccessCounter),
                new IsolatedCounter(CounterChangeInfo.FailsKey, this.swarmCoordinator.Status.FailCounter),
                new IsolatedCounter(CounterChangeInfo.ConnectionsKey, this.swarmCoordinator.Status.ConnectionCounter),
                new IsolatedCounter(CounterChangeInfo.ProxiesKey, this.swarmCoordinator.Status.ProxyCounter)
            };

            
        }

        public void Start()
        {
            this.swarmCoordinator.Start();
        }

        public void Pause()
        {
            this.swarmCoordinator.Pause();
        }

        public void ReadProxiesFromFiles(string[] fileNames)
        {
            foreach (var file in fileNames)
            {
                this.proxySource.ReadProxiesFromFileAsync(file, CancellationToken.None);
            }
        }

        public void NotifyOnCounterChange(MarshaledResultSetter<CounterChangeInfo> callback)
        {
            Task.WhenAny(this.counters.Select(c => c.ReceiveAsync()))
                .ContinueWith(delegate
                {
                    callback.SetResult(new CounterChangeInfo
                    {
                        NewValues = this.counters
                            .Select(c => new { c.Key, Result = c.ReturnAndFlushIfReceived() })
                            .Where(x => x.Result != null)
                            .Select(x => new KeyValuePair<string, int>(x.Key, x.Result.Value))
                            .ToList()
                    });
                });
        }

        #region IDisposable
        public void Dispose()
        {
            if (this.swarmCoordinator != null)
            {
                this.swarmCoordinator.Dispose();
            }
        }
        #endregion //IDisposable
    }
}
