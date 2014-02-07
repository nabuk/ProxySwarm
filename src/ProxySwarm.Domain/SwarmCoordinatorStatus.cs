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
            this.SuccessCount = new Publisher<int>();
            this.FailCount = new Publisher<int>();
            this.TaskCount = new Publisher<int>();
            this.ProxyCount = new Publisher<int>();
        }

        public Publisher<int> SuccessCount { get; private set; }
        public Publisher<int> FailCount { get; private set; }
        public Publisher<int> TaskCount { get; private set; }
        public Publisher<int> ProxyCount { get; private set; }
    }
}
