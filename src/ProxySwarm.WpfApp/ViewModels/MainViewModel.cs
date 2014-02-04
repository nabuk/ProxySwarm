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

        private void FilesPickedHandler(string[] fileNames)
        {
            throw new NotImplementedException();
        }

        public MainViewModel()
        {
            this.PlayPauseCommand = new DelegateCommand(this.PlayPauseHandler);
            this.FilesPickedCommand = new DelegateCommand<string[]>(this.FilesPickedHandler);
        }

        public ICommand PlayPauseCommand { get; private set; }

        public ICommand FilesPickedCommand { get; private set; }
    }
}
