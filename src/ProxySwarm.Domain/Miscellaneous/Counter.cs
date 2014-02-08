using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySwarm.Domain.Miscellaneous
{
    public class Counter
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

        public int Count { get { return this.count; } }

        public async Task<int> ReceiveAsync(CancellationToken token = default(CancellationToken))
        {
            return await this.buffer.ReceiveAsync(token);
        }
    }
}
