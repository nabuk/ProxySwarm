using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProxySwarm.Domain.ProxyFactory
{
    public class DefaultProxyFactory : IProxyFactory
    {
        public IList<Proxy> RetrieveProxies(string content)
        {
            return Regex.Matches(content, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}")
                .OfType<Match>()
                .Select(MapMatchToProxy)
                .ToArray();
        }

        private static Proxy MapMatchToProxy(Match match)
        {
            var parts = match.Value.Split(':');
            return new Proxy(parts[0], int.Parse(parts[1]));
        }
    }
}
