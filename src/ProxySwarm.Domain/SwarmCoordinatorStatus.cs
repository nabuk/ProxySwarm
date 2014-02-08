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
            this.SuccessCount = new RecentBuffer<int>();
            this.FailCount = new RecentBuffer<int>();
            this.TaskCount = new RecentBuffer<int>();
            this.ProxyCount = new RecentBuffer<int>();
        }

        public RecentBuffer<int> SuccessCount { get; private set; }
        public RecentBuffer<int> FailCount { get; private set; }
        public RecentBuffer<int> TaskCount { get; private set; }
        public RecentBuffer<int> ProxyCount { get; private set; }
    }
}
