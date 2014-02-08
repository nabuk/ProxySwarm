using ProxySwarm.Domain.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.WpfApp.Core
{
    public class CounterBind : ProxySwarm.WpfApp.Core.ICounterBind
    {
        private readonly Action<int> updatePropertyCallback;
        private readonly Counter counter;

        private Task<int> receiveAsync;

        public CounterBind(Action<int> updatePropertyCallback, Counter counter)
        {
            this.updatePropertyCallback = updatePropertyCallback;
            this.counter = counter;
            this.receiveAsync = this.counter.ReceiveAsync();
        }

        public async Task ReceiveAsync()
        {
            await this.receiveAsync;
        }

        public void UpdateAndFlushIfReceived()
        {
            if (!this.receiveAsync.IsCompleted)
                return;

            this.updatePropertyCallback(this.receiveAsync.Result);

            this.receiveAsync = this.counter.ReceiveAsync();
        }
    }
}
