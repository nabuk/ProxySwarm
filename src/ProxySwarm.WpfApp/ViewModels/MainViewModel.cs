using PropertyChanged;
using ProxySwarm.Domain;
using ProxySwarm.Domain.Isolation;
using ProxySwarm.WpfApp.Concrete;
using ProxySwarm.WpfApp.Core;
using ProxySwarm.WpfApp.Properties;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProxySwarm.WpfApp.ViewModels
{
    [ImplementPropertyChanged]
    public class MainViewModel : BaseViewModel
    {
        private readonly Coordinator coordinator;
        private readonly Dictionary<string, Action<int>> counterUpdateMap;
        private bool isPlaying;

        private void PlayPauseHandler()
        {
            if (this.isPlaying)
                this.coordinator.Pause();
            else
                this.coordinator.Start();

            this.isPlaying = !this.isPlaying;
        }

        private void FilesPickedHandler(string[] fileNames)
        {
            this.coordinator.ReadProxiesFromFiles(fileNames);
        }

        private async Task UpdateUIAsync()
        {
            while (true)
            {
                var counterChangeTask = this.coordinator.GetCounterChangeAsync();
                await Task.WhenAll(this.uiInvoker.YieldBackgroundPriority(), Task.Delay(50), counterChangeTask);

                foreach (var change in counterChangeTask.Result.NewValues)
                    this.counterUpdateMap[change.Key](change.Value);
            }
        }

        public MainViewModel(IUIInvoker uiInvoker, int maxConnectionCount)
            : base(uiInvoker)
        {
            this.PlayPauseCommand = new DelegateCommand(this.PlayPauseHandler);
            this.FilesPickedCommand = new DelegateCommand<string[]>(this.FilesPickedHandler);
            this.counterUpdateMap = new Dictionary<string, Action<int>>
            {
                { CounterChangeInfo.SuccessesKey, x => this.SuccessCount = x },
                { CounterChangeInfo.FailsKey, x => this.FailCount = x },
                { CounterChangeInfo.ConnectionsKey, x => this.ConnectionCount = x },
                { CounterChangeInfo.ProxiesKey, x => this.ProxyCount = x }
            };
            this.coordinator = new Coordinator(
                new CoordinatorCreateOptions
                {
                    MaxWorkerCount = maxConnectionCount,
                    ProxyWorkerFactoryType = typeof(TestProxyWorkerFactory)
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