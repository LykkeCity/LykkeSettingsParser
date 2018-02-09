using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class TcpCheckAttribute : BaseCheckAttribute
    {
        public TcpCheckAttribute(string port = null)
        {
            PortName = port;
        }
        
        public TcpCheckAttribute(int port)
        {
            Port = port;
        }

        public string PortName { get; }
        public int Port { get; }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new TcpChecker(PortName, Port);
        }
    }
}
