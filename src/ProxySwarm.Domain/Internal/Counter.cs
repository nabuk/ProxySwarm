using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySwarm.Internal.Domain
{
    internal class Counter
    {
        private readonly object locker = new object();
        private readonly RecentBuffer<int> buffer = new RecentBuffer<int>();
        private int count;

        internal void Increment()
        {
            lock (locker)
                this.buffer.Post(++count);
        }

        internal void Decrement()
        {
            lock (locker)
                this.buffer.Post(--count);
        }

        internal int Count { get { return this.count; } }

        internal async Task<int> ReceiveAsync(CancellationToken token = default(CancellationToken))
        {
            return await this.buffer.ReceiveAsync(token);
        }
    }
}
