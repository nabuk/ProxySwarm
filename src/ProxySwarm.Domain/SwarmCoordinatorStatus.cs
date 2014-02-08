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
            this.SuccessCount = new Notifier<int>();
            this.FailCount = new Notifier<int>();
            this.TaskCount = new Notifier<int>();
            this.ProxyCount = new Notifier<int>();
        }

        public Notifier<int> SuccessCount { get; private set; }
        public Notifier<int> FailCount { get; private set; }
        public Notifier<int> TaskCount { get; private set; }
        public Notifier<int> ProxyCount { get; private set; }
    }
}
