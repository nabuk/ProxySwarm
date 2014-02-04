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
        protected void RaisePropertyChanged([CallerMemberName] string callerMemberName = null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(callerMemberName));
        }
        
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
