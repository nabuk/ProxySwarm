using ProxySwarm.WpfApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProxySwarm.WpfApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private void PlayPauseHandler()
        {
            throw new NotImplementedException();
        }

        public MainViewModel()
        {
            this.PlayPauseCommand = new DelegateCommand(this.PlayPauseHandler);
        }

        public ICommand PlayPauseCommand { get; private set; }
    }
}
