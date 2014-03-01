using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.Domain.Logic
{
    internal class SwarmCoordinatorStatus
    {
        internal SwarmCoordinatorStatus()
        {
            this.SuccessCounter = new Counter();
            this.FailCounter = new Counter();
            this.ConnectionCounter = new Counter();
            this.ProxyCounter = new Counter();
        }

        internal Counter SuccessCounter { get; set; }
        internal Counter FailCounter { get; set; }
        internal Counter ConnectionCounter { get; set; }
        internal Counter ProxyCounter { get; set; }
    }
}
