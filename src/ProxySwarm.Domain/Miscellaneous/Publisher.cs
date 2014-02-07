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

        private T data;
        private Task publishTask;

        private void PublishWorkerMethod()
        {
            while (!this.cancellationToken.IsCancellationRequested)
            {
                throw new NotImplementedException();
            }
        }

        internal Publisher(TimeSpan delay, CancellationToken cancellationToken)
        {
            this.delay = delay;
            this.cancellationToken = cancellationToken;
            this.publishTask = Task.Run((Action)PublishWorkerMethod, cancellationToken);
        }

        internal void Publish(T data)
        {
            throw new NotImplementedException();
        }

        internal event Action<T> NewDataAvailable = delegate { };
    }
}
