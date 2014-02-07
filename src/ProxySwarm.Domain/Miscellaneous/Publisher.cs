using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySwarm.Domain.Miscellaneous
{
    class Publisher<T>
    {
        private readonly TimeSpan delay;
        private readonly CancellationToken cancellationToken;
        private readonly Task cancellationTask;

        private T data;

        private bool published = true;
        private readonly object publishLock = new object();

        private async void PublishMethod()
        {
            await Task.WhenAny(Task.Delay(this.delay), this.cancellationTask);
            this.cancellationToken.ThrowIfCancellationRequested();

            T dataToPublish;
            lock (this.publishLock)
            {
                this.published = true;
                dataToPublish = this.data;
            }
            this.NewDataAvailable(dataToPublish);
        }

        internal Publisher(TimeSpan delay, CancellationToken cancellationToken)
        {
            this.delay = delay;
            this.cancellationToken = cancellationToken;
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(() => tcs.TrySetCanceled(), useSynchronizationContext: false);
            this.cancellationTask = tcs.Task;
        }

        internal void Publish(T data)
        {
            var runNewTask = false;

            lock (this.publishLock)
            {
                this.data = data;
                if (this.published)
                {
                    this.published = false;
                    runNewTask = true;
                }
            }

            if (runNewTask)
                Task.Run((Action)this.PublishMethod);
        }

        internal event Action<T> NewDataAvailable = delegate { };
    }
}
