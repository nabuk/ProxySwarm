using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.Domain.Isolation
{
    [Serializable]
    public class CounterChangeInfo
    {
        public const string SuccessesKey = "successes";
        public const string FailsKey = "fails";
        public const string ConnectionsKey = "connections";
        public const string ProxiesKey = "proxies";

        public List<KeyValuePair<string, int>> NewValues { get; set; }
    }
}
