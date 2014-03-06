using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            var cmd = this.FilesPickedCommand;
            if (cmd != null && cmd.CanExecute(fileNames))
                cmd.Execute(fileNames);
        }

        public static readonly DependencyProperty FilesPickedCommandProperty = DependencyProperty.Register(
          "FilesPickedCommand",
          typeof(ICommand),
          typeof(DropFilesControl),
          new FrameworkPropertyMetadata(null));

        public DropFilesControl()
        {
            InitializeComponent();
        }

        public event FilesPickedEventHandler FilesPicked
        {
            add { AddHandler(FilesPickedEvent, value); }
            remove { RemoveHandler(FilesPickedEvent, value); }
        }

        public ICommand FilesPickedCommand
        {
            get { return (ICommand)GetValue(FilesPickedCommandProperty); }
            set { SetValue(FilesPickedCommandProperty, value); }
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
}
