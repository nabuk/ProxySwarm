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
            Func<Match, Proxy> mapToProxyObject = m =>
            {
                var parts = m.Value.Split(':');
                return new Proxy(parts[0], int.Parse(parts[1]));
            };

            return Regex.Matches(content, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}:\d{1,5}")
                .OfType<Match>()
                .Select(mapToProxyObject)
                .ToArray();
        }
    }
}
