using ProxySwarm.Domain.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.Domain.Isolation
{
    internal class IsolatedCounter
    {
        private readonly Counter counter;
        private Task<int> receiveAsync;

        internal IsolatedCounter(string key, Counter counter)
        {
            this.Key = key;
            this.counter = counter;
            this.receiveAsync = this.counter.ReceiveAsync();
        }

        internal string Key { get; private set; }

        internal async Task ReceiveAsync()
        {
            await this.receiveAsync;
        }

        internal int? ReturnAndFlushIfReceived()
        {
            if (!this.receiveAsync.IsCompleted)
                return null;

            int result = this.receiveAsync.Result;

            this.receiveAsync = this.counter.ReceiveAsync();

            return result;
        }
    }
}
