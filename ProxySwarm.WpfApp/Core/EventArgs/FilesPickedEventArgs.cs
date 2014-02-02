using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProxySwarm.WpfApp.Core.EventArgs
{
    public class FilesPickedEventArgs : RoutedEventArgs
    {
        public FilesPickedEventArgs(RoutedEvent routedEvent, string[] fileNames)
            : base(routedEvent)
        {
            this.FileNames = fileNames;
        }

        public string[] FileNames { get; private set; }
    }

    public delegate void FilesPickedEventHandler(object sender, FilesPickedEventArgs e);
}
