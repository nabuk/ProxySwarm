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
        public SwarmCoordinatorStatus()
        {
            this.SuccessCounter = new Counter();
            this.FailCounter = new Counter();
            this.TaskCounter = new Counter();
            this.ProxyCounter = new Counter();
        }

        public Counter SuccessCounter { get; private set; }
        public Counter FailCounter { get; private set; }
        public Counter TaskCounter { get; private set; }
        public Counter ProxyCounter { get; private set; }
    }
}
