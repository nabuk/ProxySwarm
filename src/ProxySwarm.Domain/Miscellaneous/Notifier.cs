using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySwarm.Domain.Miscellaneous
{
    public class Notifier<T>
    {
        private readonly object dataLocker = new object();

        private T data;
        private bool hasNewData;
        private TaskCompletionSource<T> dataTaskCompletionSource;

        internal void Publish(T data)
        {
            TaskCompletionSource<T> tcs = null;
            lock (dataLocker)
            {
                tcs = this.dataTaskCompletionSource;
                if (tcs == null)
                {
                    this.data = data;
                    this.hasNewData = true;
                }
                else
                {
                    this.hasNewData = false;
                    this.dataTaskCompletionSource = null;
                }
            }
            if (tcs != null)
                tcs.TrySetResult(data);
        }

        public Task<T> GetNewDataAsync()
        {
            lock (this.dataLocker)
            {
                if (this.hasNewData)
                {
                    this.hasNewData = false;
                    return Task.FromResult(this.data);
                }
                else
                {
                    if (this.dataTaskCompletionSource == null)
                        this.dataTaskCompletionSource = new TaskCompletionSource<T>();

                    return this.dataTaskCompletionSource.Task;
                }
            }
        }
    }
}
