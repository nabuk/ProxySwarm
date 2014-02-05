using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.Domain
{
    public class ProxyBag
    {
        private readonly ConcurrentDictionary<Proxy, byte> dictionary = new ConcurrentDictionary<Proxy, byte>();
        private readonly ConcurrentBag<Proxy> bag = new ConcurrentBag<Proxy>();

        public bool Add(Proxy proxy)
        {
            if (this.dictionary.TryAdd(proxy, 0))
            {
                this.bag.Add(proxy);
                return true;
            }
            else
                return false;
        }

        public Proxy Pop()
        {
            Proxy result;
            return this.bag.TryTake(out result) ? result : null;
        }

        public int Count
        {
            get
            {
                return this.bag.Count;
            }
        }
    }
}
