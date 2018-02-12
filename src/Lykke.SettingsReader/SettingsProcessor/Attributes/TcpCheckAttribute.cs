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
            : base(true)
        {
            PortName = port;
        }

        /// <summary>
        /// C-tor with other property name for port value and throwExceptionOnFail flag
        /// </summary>
        /// <param name="port">Port property name</param>
        /// <param name="throwExceptionOnFail">Throw exception on fail flag</param>
        public TcpCheckAttribute(string port, bool throwExceptionOnFail)
            : base(throwExceptionOnFail)
        {
            PortName = port;
        }

        /// <summary>
        /// C-tor with explicit port value
        /// </summary>
        /// <param name="port">Explicit port value</param>
        public TcpCheckAttribute(int port)
            : base(true)
        {
            Port = port;
        }

        /// <summary>
        /// C-tor with explicit port value and throwExceptionOnFail flag
        /// </summary>
        /// <param name="port">Explicit port value</param>
        /// <param name="throwExceptionOnFail">Throw exception on fail flag</param>
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
