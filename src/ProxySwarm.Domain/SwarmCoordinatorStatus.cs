using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.Domain
{
    public class SwarmCoordinatorStatus
    {
        public SwarmCoordinatorStatus()
        {
            this.SuccessCounter = new Counter();
            this.FailCounter = new Counter();
            this.ConnectionCounter = new Counter();
            this.ProxyCounter = new Counter();
        }

        public Counter SuccessCounter { get; internal set; }
        public Counter FailCounter { get; internal set; }
        public Counter ConnectionCounter { get; internal set; }
        public Counter ProxyCounter { get; internal set; }
    }
}
