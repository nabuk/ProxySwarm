using System;

namespace ProxySwarm.Domain
{
    public sealed class Proxy : IEquatable<Proxy>
    {
        public Proxy(string host, int port)
        {
            if (string.IsNullOrWhiteSpace(host)) throw new ArgumentException("host cannot be null or whitespace.");

            this.Host = host;
            this.Port = port;
        }

        public string Host { get; private set; }

        public int Port { get; private set; }

        #region Overrides
        public override string ToString()
        {
            return string.Format("{0} : {1}", Host, Port);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Proxy);
        }

        public override int GetHashCode()
        {
            return Host.GetHashCode();
        }
        #endregion //Overrides

        #region IEquatable<Proxy>
        public bool Equals(Proxy other)
        {
            if (other == null)
                return false;

            return Host == other.Host && Port == other.Port;
        }
        #endregion //IEquatable<Proxy>
    }
}
