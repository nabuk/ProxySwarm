using ProxySwarm.WpfApp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.WpfApp.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private readonly IUIInvoker uiInvoker;

        public BaseViewModel(IUIInvoker uiInvoker)
        {
            if (uiInvoker == null)
                throw new ArgumentNullException("uiInvoker");

            this.uiInvoker = uiInvoker;
        }

        protected void RaisePropertyChanged([CallerMemberName] string callerMemberName = null)
        {
            this.uiInvoker.InvokeOnUIThreadAsync(() => this.PropertyChanged(this, new PropertyChangedEventArgs(callerMemberName)));
        }
        
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
