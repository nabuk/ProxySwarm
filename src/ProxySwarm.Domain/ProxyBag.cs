using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ProxySwarm.Domain
{
    public class ProxyBag
    {
        private readonly ConcurrentDictionary<Proxy, byte> dictionary = new ConcurrentDictionary<Proxy, byte>();
        private readonly BufferBlock<Proxy> buffer;

        public ProxyBag(CancellationToken cancellationToken)
        {
            this.buffer = new BufferBlock<Proxy>(new DataflowBlockOptions { CancellationToken = cancellationToken });
        }

        public bool Add(Proxy proxy)
        {
            if (this.dictionary.TryAdd(proxy, 0))
            {
                this.buffer.Post(proxy);
                return true;
            }
            else
                return false;
        }

        public Task<Proxy> GetAsync()
        {
            return this.buffer.ReceiveAsync();
        }

        public int Count
        {
            get
            {
                return this.buffer.Count;
            }
        }
    }
}
