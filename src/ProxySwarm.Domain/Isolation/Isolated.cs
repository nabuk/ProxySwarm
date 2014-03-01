using System;

namespace ProxySwarm.Domain.Isolation
{
    //http://www.superstarcoders.com/blogs/posts/executing-code-in-a-separate-application-domain-using-c-sharp.aspx

    public sealed class Isolated<T> : IDisposable where T : MarshalByRefObject
    {
        private AppDomain domain;
        private T value;

        public Isolated()
        {
            this.domain = AppDomain.CreateDomain("Isolated:" + Guid.NewGuid(), null, AppDomain.CurrentDomain.SetupInformation);

            var type = typeof(T);
            this.value = (T)this.domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        public T Value
        {
            get
            {
                return this.value;
            }
        }

        public void Dispose()
        {
            if (this.domain != null)
            {
                AppDomain.Unload(this.domain);
                this.domain = null;
            }
        }
    }
}
