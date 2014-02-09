using ProxySwarm.Domain;
using ProxySwarm.Domain.Miscellaneous;
using ProxySwarm.Domain.ProxyFactory;
using ProxySwarm.WpfApp.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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


            var maxConnectionsCount = 1000;
            this.proxyFileSource = new ProxyFileSource(new DefaultProxyFactory());
            this.swarmCoordinator = new SwarmCoordinator(new FakeProxyWorkerFactory(), proxyFileSource, maxConnectionsCount);

            System.Net.ServicePointManager.Expect100Continue = false;
            System.Net.ServicePointManager.DefaultConnectionLimit = maxConnectionsCount;

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
                        await Task.WhenAll(this.uiInvoker.YieldBackgroundPriority(), Task.Delay(500));
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
                var request = System.Net.WebRequest.CreateHttp("http://www.google.com");
                request.Method = "GET";
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:12.0) Gecko/20100101 Firefox/12.0";
                request.Timeout = 20000;
                request.ContentLength = 0;
                request.Proxy = new System.Net.WebProxy(proxy.Address, proxy.Port);
                var requestTime = DateTime.UtcNow;

                var cts = new CancellationTokenSource();
                var cancellationToken = cts.Token;
                cts.CancelAfter(request.Timeout);

                try
                {
                    var cancellationTcs = new TaskCompletionSource<bool>();

                    using (cancellationToken.Register(() => cancellationTcs.SetCanceled(), useSynchronizationContext: false))
                    using (var ms = new System.IO.MemoryStream())
                    {
                        System.Net.WebResponse response = null;
                        try
                        {
                            var webResponseTask = request.GetResponseAsync();
                            await Task.WhenAny(webResponseTask, cancellationTcs.Task);
                            response = webResponseTask.Result;

                            using (var responseStream = response.GetResponseStream())
                            {
                                if (responseStream == null)
                                    throw new System.Data.NoNullAllowedException("Response stream cannot be null.");

                                var buffer = new byte[1024 * 64];
                                var readCount = -1;
                                while (readCount != 0)
                                {
                                    var readTask = responseStream.ReadAsync(buffer, 0, 1024 * 64, cancellationToken);
                                    await Task.WhenAny(readTask, cancellationTcs.Task);
                                    readCount = readTask.Result;
                                    await ms.WriteAsync(buffer, 0, readCount);
                                }
                            }

                            ms.Seek(0, System.IO.SeekOrigin.Begin);

                            using (var sr = new System.IO.StreamReader(ms))
                                await sr.ReadToEndAsync();
                        }
                        finally
                        {
                            if (response != null)
                                response.Dispose();
                        }
                    }
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }
    }
}