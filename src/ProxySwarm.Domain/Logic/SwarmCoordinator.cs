using ProxySwarm.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySwarm.Domain.Logic
{
    internal class SwarmCoordinator : IDisposable
    {
        private readonly IProxyWorkerFactory proxyWorkerFactory;
        private readonly int maxWorkerCount;
        private readonly ProxyBag proxyBag;
 
        private TaskCompletionSource<bool> isRunningCompletionSource = new TaskCompletionSource<bool>();
        private Task isRunningTask;
        private CancellationTokenSource disposeCancellationSource = new CancellationTokenSource();
        private Task throttlerTask;

        internal SwarmCoordinator(IProxyWorkerFactory proxyWorkerFactory, IObservable<Proxy> proxySource, int maxWorkerCount)
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
 
        private async Task ThrottlerMethod(CancellationToken cancellationToken)
        {
            using (var throttler = new SemaphoreSlim(initialCount: this.maxWorkerCount, maxCount: this.maxWorkerCount))
                while (!cancellationToken.IsCancellationRequested)
                {
                    await throttler.WaitAsync();
                    var proxy = await this.proxyBag.ReceiveAsync(cancellationToken);
 
                    await this.isRunningTask;
 
                    Task.Run(async () =>
                    {
                        try { await WorkerMethod(proxy); }
                        finally { throttler.Release(); }
                    });
                }
        }

        internal void Start()
        {
            if (this.isRunningTask.IsCompleted)
                return;
 
            this.isRunningCompletionSource.SetResult(true);
 
            if (this.throttlerTask == null)
                this.throttlerTask = Task.Factory.StartNew(async () => await this.ThrottlerMethod(this.disposeCancellationSource.Token), TaskCreationOptions.LongRunning);
        }

        internal void Pause()
        {
            if (!this.isRunningTask.IsCompleted)
                return;
 
            this.isRunningCompletionSource = new TaskCompletionSource<bool>();
            this.isRunningTask = this.isRunningCompletionSource.Task;
        }

        internal SwarmCoordinatorStatus Status { get; private set; }
 
        #region IDisposable
        public void Dispose()
        {
            this.disposeCancellationSource.Cancel();
            this.proxyBag.Dispose();
        }
        #endregion //IDisposable
    }
}