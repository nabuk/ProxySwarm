using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxySwarm.Domain.Isolation
{
    public class MarshaledResultSetter<T> : MarshalByRefObject
    {
        private readonly TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

        public void SetResult(T result)
        {
            this.tcs.SetResult(result);
        }

        public Task<T> Task
        {
            get
            {
                return this.tcs.Task;
            }
        }
    }
}
