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
        private readonly int delay;
        private readonly CancellationToken cancellationToken;

        private T toPublish;
        private Task PublishTask;

        private void PublishWorkerMethod()
        {
            while (this.cancellationToken.IsCancellationRequested)
            {
                throw new NotImplementedException();
            }
        }

        internal Publisher(int delay, CancellationToken cancellationToken)
        {
            this.delay = delay;
            this.cancellationToken = cancellationToken;
            this.PublishTask = Task.Run((Action)PublishWorkerMethod, cancellationToken);
        }

        internal void Publish(T data)
        {
            throw new NotImplementedException();
        }

        internal event Action<T> NewData = delegate { };
    }
}
