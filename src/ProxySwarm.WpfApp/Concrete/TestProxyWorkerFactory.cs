using ProxySwarm.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySwarm.WpfApp.Concrete
{
    public class TestProxyWorkerFactory : IProxyWorkerFactory
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
