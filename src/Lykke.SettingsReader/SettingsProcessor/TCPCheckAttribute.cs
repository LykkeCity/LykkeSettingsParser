using System;

namespace Lykke.SettingsReader.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    // ReSharper disable once InconsistentNaming
    public class TcpCheckAttribute : Attribute
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

        public bool IsPortProvided => !string.IsNullOrEmpty(PortName) || Port > 0;
    }
}
