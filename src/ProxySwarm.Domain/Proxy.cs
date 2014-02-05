using System;

namespace ProxySwarm.Domain
{
    public class Proxy : IEquatable<Proxy>
    {
        public Proxy(string address, int port)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException("address cannot be null or whitespace.");

            this.Address = address;
            this.Port = port;
        }

        public string Address { get; private set; }
        public int Port { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} : {1}", Address ?? "NULL address", Port);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Proxy);
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }

        public bool Equals(Proxy other)
        {
            if (other == null)
                return false;

            return Address == other.Address && Port == other.Port;
        }
    }
}
