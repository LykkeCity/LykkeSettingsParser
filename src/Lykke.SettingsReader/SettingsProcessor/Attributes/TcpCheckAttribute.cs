using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class TcpCheckAttribute : BaseCheckAttribute
    {
        public string PortName { get; }

        public int Port { get; }

        public TcpCheckAttribute(string port = null)
            : base(true)
        {
            PortName = port;
        }

        public TcpCheckAttribute(string port, bool throwExceptionOnFail)
            : base(throwExceptionOnFail)
        {
            PortName = port;
        }

        public TcpCheckAttribute(int port)
            : base(true)
        {
            Port = port;
        }

        public TcpCheckAttribute(int port, bool throwExceptionOnFail)
            : base(throwExceptionOnFail)
        {
            Port = port;
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new TcpChecker(PortName, Port, _throwExceptionOnFail);
        }
    }
}
