using Microsoft.Win32;
using ProxySwarm.WpfApp.Core.EventArgs;
using System.Windows;
using System.Windows.Controls;

namespace ProxySwarm.WpfApp.Controls
{
    /// <summary>
    /// Interaction logic for DropFilesControl.xaml
    /// </summary>
    public partial class DropFilesControl : UserControl
    {
        public static readonly RoutedEvent FilesPickedEvent = EventManager.RegisterRoutedEvent(
            "FilesPicked", RoutingStrategy.Bubble, typeof(FilesPickedEventHandler), typeof(DropFilesControl));

        private void RaiseFilesPickedEvent(string[] fileNames)
        {
            var eventArgs = new FilesPickedEventArgs(DropFilesControl.FilesPickedEvent, fileNames);
            RaiseEvent(eventArgs);
        }

        public DropFilesControl()
        {
            InitializeComponent();
        }

        public event FilesPickedEventHandler FilesPicked
        {
            add { AddHandler(FilesPickedEvent, value); }
            remove { RemoveHandler(FilesPickedEvent, value); }
        }

        private void BrowseFilesLink_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Multiselect = true
            };

            if (ofd.ShowDialog() == true)
                this.RaiseFilesPickedEvent(ofd.FileNames);
        }

        private void DropFilesControl_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
                this.RaiseFilesPickedEvent(fileNames);
            }
        }
    }
}
