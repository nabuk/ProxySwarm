using ProxySwarm.Domain;
using ProxySwarm.Domain.Miscellaneous;
using ProxySwarm.Domain.ProxyFactory;
using ProxySwarm.WpfApp.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProxySwarm.WpfApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ICommand playPauseCommand;
        private readonly ICommand filesPickedCommand;
        private int successCount;
        private int failCount;
        private int connectionCount;
        private int proxyCount;

        private readonly ProxyFileSource proxyFileSource;
        private readonly SwarmCoordinator swarmCoordinator;

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

        public MainViewModel(IUIInvoker uiInvoker) : base(uiInvoker)
        {
            this.playPauseCommand = new DelegateCommand(this.PlayPauseHandler);
            this.filesPickedCommand = new DelegateCommand<string[]>(this.FilesPickedHandler);


            this.proxyFileSource = new ProxyFileSource(new DefaultProxyFactory());
            this.swarmCoordinator = new SwarmCoordinator(new FakeProxyWorkerFactory(), proxyFileSource, 100);

            this.uiInvoker.InvokeOnUIThreadAsync(
                async () =>
                {
                    var status = this.swarmCoordinator.Status;
                    ICounterBind counterBinds = new CounterBindAggregate(
                        new[]
                        {
                            new CounterBind(x => this.SuccessCount = x, status.SuccessCounter),
                            new CounterBind(x => this.FailCount = x, status.FailCounter),
                            new CounterBind(x => this.ConnectionCount = x, status.ConnectionCounter),
                            new CounterBind(x => this.ProxyCount = x, status.ProxyCounter)
                        });
                    
                    while (true)
                    {
                        await this.uiInvoker.YieldBackgroundPriority();
                        await counterBinds.ReceiveAsync();
                        counterBinds.UpdateAndFlushIfReceived();
                    }
                });
        }

        public ICommand PlayPauseCommand { get { return this.playPauseCommand; } }
        public ICommand FilesPickedCommand { get { return this.filesPickedCommand; } }

        public int SuccessCount
        {
            get
            {
                return this.successCount;
            }
            set
            {
                this.successCount = value;
                RaisePropertyChanged();
            }
        }
        public int FailCount
        {
            get
            {
                return this.failCount;
            }
            set
            {
                this.failCount = value;
                RaisePropertyChanged();
            }
        }
        public int ConnectionCount
        {
            get
            {
                return this.connectionCount;
            }
            set
            {
                this.connectionCount = value;
                RaisePropertyChanged();
            }
        }
        public int ProxyCount
        {
            get
            {
                return this.proxyCount;
            }
            set
            {
                this.proxyCount = value;
                RaisePropertyChanged();
            }
        }

        class FakeProxyWorkerFactory : IProxyWorkerFactory
        {
            public async Task<bool> CreateWorkerAsync(Proxy proxy)
            {
                await Task.Delay(500).ConfigureAwait(false);
                return true;
            }
        }

        
    }
}
