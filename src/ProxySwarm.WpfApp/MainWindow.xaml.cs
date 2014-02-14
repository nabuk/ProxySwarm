using ProxySwarm.Domain;
using ProxySwarm.WpfApp.Concrete;
using ProxySwarm.WpfApp.Core;
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

            var maxConnectionCount = 150;
            System.Net.ServicePointManager.Expect100Continue = false;
            System.Net.ServicePointManager.DefaultConnectionLimit = maxConnectionCount;
            
            var proxyFileSource = new ProxyFileSource(new DefaultProxyFactory());
            var swarmCoordinator = new SwarmCoordinator(new TestProxyWorkerFactory(), proxyFileSource, maxConnectionCount);

            this.DataContext = new MainViewModel(swarmCoordinator, proxyFileSource, new UIInvoker(this.Dispatcher));
        }
    }
}
