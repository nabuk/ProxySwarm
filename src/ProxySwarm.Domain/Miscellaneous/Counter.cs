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
        private int count;

        public Counter()
        {
            this.Notifier = new RecentBuffer<int>();
        }

        internal void Increment()
        {
            lock (locker)
                Notifier.Post(++count);
        }

        internal void Decrement()
        {
            lock (locker)
                Notifier.Post(--count);
        }

        public int Count { get { return this.count; } }

        public RecentBuffer<int> Notifier { get; private set; }
    }
}
