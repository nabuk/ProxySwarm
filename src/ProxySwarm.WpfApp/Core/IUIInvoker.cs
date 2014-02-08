using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.WpfApp.Core
{
    public interface IUIInvoker
    {
        Task InvokeOnUIThreadAsync(Action action);
        Task YieldBackgroundPriority();
    }
}
