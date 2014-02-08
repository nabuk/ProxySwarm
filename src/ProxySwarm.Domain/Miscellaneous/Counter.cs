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
            this.Notifier = new Notifier<int>();
        }

        internal void Increment()
        {
            lock (locker)
                Notifier.Publish(++count);
        }

        internal void Decrement()
        {
            lock (locker)
                Notifier.Publish(--count);
        }

        public int Count { get { return this.count; } }

        public Notifier<int> Notifier { get; private set; }
    }
}
