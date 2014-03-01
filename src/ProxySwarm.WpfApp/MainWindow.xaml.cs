using ProxySwarm.Domain;
using ProxySwarm.WpfApp.Concrete;
using ProxySwarm.WpfApp.Core;
using ProxySwarm.WpfApp.Properties;
using ProxySwarm.WpfApp.ViewModels;
using System.Windows;

namespace ProxySwarm.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var maxConnectionCount = Settings.Default.MaxConnectionCount;
            System.Net.ServicePointManager.Expect100Continue = false;
            System.Net.ServicePointManager.DefaultConnectionLimit = maxConnectionCount;
            
            this.DataContext = new MainViewModel(new UIInvoker(this.Dispatcher));
        }
    }
}
