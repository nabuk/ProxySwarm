using PropertyChanged;
using ProxySwarm.Domain;
using ProxySwarm.WpfApp.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProxySwarm.WpfApp.ViewModels
{
    [ImplementPropertyChanged]
    public class MainViewModel : BaseViewModel
    {
        private readonly SwarmCoordinator swarmCoordinator;
        private readonly ProxyFileSource proxyFileSource;
        private readonly ICounterBind counterBinds;

        private bool isPlaying;

        private void PlayPauseHandler()
        {
            if (this.isPlaying)
                this.swarmCoordinator.Pause();
            else
                this.swarmCoordinator.Start();

            this.isPlaying = !this.isPlaying;
        }
        private void FilesPickedHandler(string[] fileNames)
        {
            foreach (var file in fileNames)
                this.proxyFileSource.ReadProxiesFromFileAsync(file, CancellationToken.None);
        }

        private async Task UpdateUIAsync()
        {
            while (true)
            {
                await Task.WhenAll(this.uiInvoker.YieldBackgroundPriority(), Task.Delay(500));
                await this.counterBinds.ReceiveAsync();
                this.counterBinds.UpdateAndFlushIfReceived();
            }
        }

        public MainViewModel(SwarmCoordinator swarmCoordinator, ProxyFileSource proxyFileSource, IUIInvoker uiInvoker)
            : base(uiInvoker)
        {
            this.PlayPauseCommand = new DelegateCommand(this.PlayPauseHandler);
            this.FilesPickedCommand = new DelegateCommand<string[]>(this.FilesPickedHandler);
            this.swarmCoordinator = swarmCoordinator;
            this.proxyFileSource = proxyFileSource;

            var status = this.swarmCoordinator.Status;
            this.counterBinds = new CounterBindAggregate(
                new[]
                        {
                            new CounterBind(x => this.SuccessCount = x, status.SuccessCounter),
                            new CounterBind(x => this.FailCount = x, status.FailCounter),
                            new CounterBind(x => this.ConnectionCount = x, status.ConnectionCounter),
                            new CounterBind(x => this.ProxyCount = x, status.ProxyCounter)
                        });

            this.uiInvoker.InvokeOnUIThreadAsync(async () => await this.UpdateUIAsync());
        }

        public ICommand PlayPauseCommand { get; private set; }
        public ICommand FilesPickedCommand { get; private set; }

        public int SuccessCount { get; private set; }
        public int FailCount { get; private set; }
        public int ConnectionCount { get; private set; }
        public int ProxyCount { get; private set; }
    }
}