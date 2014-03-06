using ProxySwarm.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace ProxySwarm.Domain.Logic
{
    internal class ProxyFileSource : IObservable<Proxy>
    {
        private readonly BufferBlock<Proxy> buffer = new BufferBlock<Proxy>();
        private readonly IObservable<Proxy> observableBuffer;
        private readonly IProxyFactory proxyFactory;

        internal ProxyFileSource(IProxyFactory proxyFactory)
        {
            this.proxyFactory = proxyFactory;
            this.observableBuffer = this.buffer.AsObservable();
        }

        internal async Task ReadProxiesFromFileAsync(string path, CancellationToken cancellationToken)
        {
            string content;

            using (var sr = new StreamReader(path))
            {
                content = await sr.ReadToEndAsync();
            }

            foreach (var proxy in proxyFactory.RetrieveProxies(content))
            {
                buffer.Post(proxy);
            }
        }

        IDisposable IObservable<Proxy>.Subscribe(IObserver<Proxy> observer)
        {
            return this.observableBuffer.Subscribe(observer);
        }
    }
}
