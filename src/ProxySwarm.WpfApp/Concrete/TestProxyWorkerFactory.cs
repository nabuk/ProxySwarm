using ProxySwarm.Domain;
using System;
using System.Net;
using System.Net.Cache;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySwarm.WpfApp.Concrete
{
    public class TestProxyWorkerFactory : IProxyWorkerFactory
    {
        public async Task<bool> CreateWorkerAsync(Proxy proxy)
        {
            try
            {
                var httpClient = new HttpClient(
                    new HttpClientHandler
                    {
                        AllowAutoRedirect = false,
                        UseCookies = true,
                        CookieContainer = new CookieContainer(),
                        Proxy = new WebProxy(proxy.Address, proxy.Port),
                        UseProxy = true
                    }
                    , true)
                {
                    Timeout = TimeSpan.FromSeconds(20),
                    MaxResponseContentBufferSize = 256000
                };

                httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:27.0) Gecko/20100101 Firefox/27.0");

                using (httpClient)
                {
                    var response = await httpClient.GetAsync("http://www.microsoft.com/en-US/default.aspx", HttpCompletionOption.ResponseContentRead);
                    using (response)
                    {
                        if (!response.IsSuccessStatusCode)
                            return false;

                        using (var content = response.Content)
                        {
                            //var strContent = await content.ReadAsStringAsync();
                            //strContent.ToString();
                        }
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
