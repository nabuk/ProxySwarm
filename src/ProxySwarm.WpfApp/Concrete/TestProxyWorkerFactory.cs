using ProxySwarm.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            request.ContinueTimeout = request.Timeout;
            request.ReadWriteTimeout = request.Timeout;
            request.ContentLength = 0;
            request.AllowAutoRedirect = false;
            request.ProtocolVersion = HttpVersion.Version11;
            request.KeepAlive = false;
            request.Proxy = new System.Net.WebProxy(proxy.Address, proxy.Port);

            var cts = new CancellationTokenSource();
            var cancellationToken = cts.Token;
            cts.CancelAfter(request.Timeout);

            var cancellationTcs = new TaskCompletionSource<bool>();

            using (cancellationToken.Register(() => cancellationTcs.SetCanceled(), useSynchronizationContext: false))
            using (var ms = new System.IO.MemoryStream())
            {
                WebResponse response = null;
                try
                {
                    var webResponseTask = request.GetResponseAsync();
                    await Task.WhenAny(webResponseTask, cancellationTcs.Task);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        request.Abort();
                        webResponseTask.ContinueWith(responseTask =>
                        {
                            if (responseTask.Status == TaskStatus.RanToCompletion)
                                responseTask.Result.Dispose();
                        });
                        
                        //await Task.WhenAny(webResponseTask);
                        //if (webResponseTask.Status == TaskStatus.RanToCompletion)
                        //    webResponseTask.Result.Dispose();
                        return false;
                    }

                    using (response = webResponseTask.Result)
                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream == null)
                            throw new System.Data.NoNullAllowedException("Response stream cannot be null.");

                        var copyTask = responseStream.CopyToAsync(ms, 1024 * 64, cancellationToken);
                        await Task.WhenAny(copyTask, cancellationTcs.Task);
                        if (cancellationToken.IsCancellationRequested)
                        {
                            await copyTask;
                            return false;
                        }
                    }

                    ms.Seek(0, System.IO.SeekOrigin.Begin);

                    using (var sr = new System.IO.StreamReader(ms))
                        await sr.ReadToEndAsync();
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
    }
}
