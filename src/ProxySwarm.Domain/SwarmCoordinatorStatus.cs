using ProxySwarm.Domain.Miscellaneous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.Domain
{
    public class SwarmCoordinatorStatus
    {
        public SwarmCoordinatorStatus(Counter proxyCounter)
        {
            this.SuccessCounter = new Counter();
            this.FailCounter = new Counter();
            this.ConnectionCounter = new Counter();
            this.ProxyCounter = proxyCounter;
        }

        public Counter SuccessCounter { get; private set; }
        public Counter FailCounter { get; private set; }
        public Counter ConnectionCounter { get; private set; }
        public Counter ProxyCounter { get; private set; }
    }
}
