using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySwarm.Domain.Miscellaneous
{
    public class RecentBuffer<T>
    {
        private readonly object locker = new object();
        private volatile TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();

        internal void Post(T data)
        {
            TaskCompletionSource<T> tempTcs;
            lock (locker)
            {
                if (this.tcs.Task.IsCompleted)
                    this.tcs = new TaskCompletionSource<T>();
                tempTcs = this.tcs;
            }
            tempTcs.SetResult(data);
        }

        public async Task<T> ReceiveAsync(CancellationToken token = default(CancellationToken))
        {
            Task<T> task = null;

            lock (locker)
                task = this.tcs.Task;

            if (token != default(CancellationToken))
            {
                var cancellationTcs = new TaskCompletionSource<bool>();
                using (token.Register(() => cancellationTcs.SetCanceled(), useSynchronizationContext: false))
                    await Task.WhenAny(task, cancellationTcs.Task).ConfigureAwait(false);

                token.ThrowIfCancellationRequested();
            }

            try
            {
                return await task.ConfigureAwait(false);
            }
            finally
            {
                lock (locker)
                    if (this.tcs.Task == task && task.IsCompleted)
                        this.tcs = new TaskCompletionSource<T>();
            }
        }
    }
}
