using System;
using System.Threading.Tasks;

namespace ProxySwarm.WpfApp.Core
{
    public interface ICounterBind
    {
        Task ReceiveAsync();
        void UpdateAndFlushIfReceived();
    }
}
