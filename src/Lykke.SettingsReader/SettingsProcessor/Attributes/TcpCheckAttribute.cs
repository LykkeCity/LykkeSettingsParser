using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Attribute for tcp connection check using provided host and port (with optional usage of another port property)
    /// </summary>
    public class TcpCheckAttribute : BaseCheckAttribute
    {
        /// <summary>Name of Port property</summary>
        public string PortName { get; }

        /// <summary>Explicit port value</summary>
        public int Port { get; }

        /// <summary>
        /// C-tor with other property name for port value
        /// </summary>
        /// <param name="port">Port property name</param>
        public TcpCheckAttribute(string port = null)
        {
            PortName = port;
        }

        /// <summary>
        /// C-tor with explicit port value
        /// </summary>
        /// <param name="port">Explicit port value</param>
        public TcpCheckAttribute(int port)
        {
            Port = port;
        }

        /// <inheritdoc />
        public override ISettingsFieldChecker GetChecker()
        {
            return new TcpChecker(PortName, Port);
        }
    }
}
