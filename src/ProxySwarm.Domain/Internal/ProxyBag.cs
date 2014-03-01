using ProxySwarm.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ProxySwarm.Internal.Domain
{
    internal class ProxyBag : IObserver<Proxy>, IDisposable
    {
        private readonly ConcurrentDictionary<Proxy, byte> dictionary = new ConcurrentDictionary<Proxy, byte>();
        private readonly BufferBlock<Proxy> buffer = new BufferBlock<Proxy>();
        private readonly IDisposable disposableSubscription;

        internal ProxyBag(IObservable<Proxy> proxySource)
        {
            this.disposableSubscription = proxySource.Subscribe(this);
            this.Counter = new Counter();
        }

        internal async Task<Proxy> ReceiveAsync(CancellationToken cancellationToken)
        {
            var result = await this.buffer.ReceiveAsync(cancellationToken);

            this.Counter.Decrement();
            return result;
        }

        internal Counter Counter { get; private set; }

        #region IDisposable
        public void Dispose()
        {
            this.disposableSubscription.Dispose();
        }
        #endregion //IDisposable

        #region IObserver<Proxy>
        void IObserver<Proxy>.OnCompleted()
        {
            
        }

        void IObserver<Proxy>.OnError(Exception error)
        {
            throw error;
        }

        void IObserver<Proxy>.OnNext(Proxy value)
        {
            if (this.dictionary.TryAdd(value, 0))
            {
                this.Counter.Increment();
                this.buffer.Post(value);
            }
        }
        #endregion //IObserver<Proxy>
    }
}
