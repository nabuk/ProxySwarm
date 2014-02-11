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
        
        private TaskCompletionSource<bool> isRunningCompletionSource = new TaskCompletionSource<bool>();
        private Task[] tasks;
        private Task isRunningTask;

        public SwarmCoordinator(IProxyWorkerFactory proxyWorkerFactory, IObservable<Proxy> proxySource, int maxWorkerCount)
        {
            this.proxyWorkerFactory = proxyWorkerFactory;
            this.maxWorkerCount = maxWorkerCount;
            this.proxyBag = new ProxyBag(proxySource);
            this.Status = new SwarmCoordinatorStatus { ProxyCounter = this.proxyBag.Counter };
            this.isRunningTask = this.isRunningCompletionSource.Task;
        }

        private async Task WorkerMethod(Proxy proxy)
        {
            this.Status.ConnectionCounter.Increment();

            var success = await this.proxyWorkerFactory.CreateWorkerAsync(proxy);
            if (success)
                this.Status.SuccessCounter.Increment();
            else
                this.Status.FailCounter.Increment();

            this.Status.ConnectionCounter.Decrement();
        }

        public void Start()
        {
            if (this.isRunningTask.IsCompleted)
                return;

            this.isRunningCompletionSource.SetResult(true);

            if (this.tasks == null)
            {
                this.tasks = new Task[this.maxWorkerCount];

                Task.Factory.StartNew(async () =>
                {
                    using (var throttler = new SemaphoreSlim(initialCount: maxWorkerCount, maxCount: this.maxWorkerCount))
                        while (true)
                        {
                            await throttler.WaitAsync();
                            var proxy = await this.proxyBag.ReceiveAsync(CancellationToken.None);

                            Task.Run(async () =>
                            {
                                try { await WorkerMethod(proxy); }
                                finally { throttler.Release(); }
                            });
                        }

                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        public void Pause()
        {
            if (!this.isRunningTask.IsCompleted)
                return;

            this.isRunningCompletionSource = new TaskCompletionSource<bool>();
            this.isRunningTask = this.isRunningCompletionSource.Task;
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
