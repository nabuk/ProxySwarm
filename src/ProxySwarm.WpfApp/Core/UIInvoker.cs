using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ProxySwarm.WpfApp.Core
{
    public class UIInvoker : IUIInvoker
    {
        private readonly Dispatcher uiDispatcher;

        public UIInvoker(Dispatcher uiDispatcher)
        {
            this.uiDispatcher = uiDispatcher;
        }

        public Task InvokeOnUIThreadAsync(Action action)
        {
            return this.uiDispatcher.InvokeAsync(action).Task;
        }
    }
}
