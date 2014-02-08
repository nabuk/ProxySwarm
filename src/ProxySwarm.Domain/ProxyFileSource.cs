using ProxySwarm.Domain.ProxyFactory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ProxySwarm.Domain
{
    public class ProxyFileSource : IObservable<Proxy>
    {
        private readonly BufferBlock<Proxy> buffer = new BufferBlock<Proxy>();
        private readonly IObservable<Proxy> observableBuffer;
        private readonly IProxyFactory proxyFactory;
        private const int DefaultBufferSize = 81920;

        public ProxyFileSource(IProxyFactory proxyFactory)
        {
            this.proxyFactory = proxyFactory;
            this.observableBuffer = this.buffer.AsObservable();
        }

        public async Task ReadProxiesFromFileAsync(string path, CancellationToken cancellationToken)
        {
            string content;

            using (var sr = new StreamReader(path))
            {
                content = await sr.ReadToEndAsync().ConfigureAwait(false);
            }

            foreach (var proxy in proxyFactory.RetrieveProxies(content))
                buffer.Post(proxy);
        }

        public IDisposable Subscribe(IObserver<Proxy> observer)
        {
            return this.observableBuffer.Subscribe(observer);
        }
    }
}
