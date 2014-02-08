using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxySwarm.WpfApp.Core
{
    public class CounterBindAggregate : ICounterBind
    {
        private readonly IList<ICounterBind> counterBinds;

        public CounterBindAggregate(IEnumerable<ICounterBind> counterBinds)
        {
            this.counterBinds = counterBinds.ToArray();
        }

        public async Task ReceiveAsync()
        {
            var receiveAsyncArray = new Task[this.counterBinds.Count];
            for (var i = 0; i < receiveAsyncArray.Length; ++i)
                receiveAsyncArray[i] = this.counterBinds[i].ReceiveAsync();

            await Task.WhenAny(receiveAsyncArray);
        }

        public void UpdateAndFlushIfReceived()
        {
            foreach (var cb in this.counterBinds)
                cb.UpdateAndFlushIfReceived();
        }
    }
}
