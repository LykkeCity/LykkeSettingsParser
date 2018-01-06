using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models
{
    public class TestTcpCheckModel
    {
        [TcpCheck]
        public string HostPort { get; set; }
        [TcpCheck("Port")]
        public string Host { get; set; }

        public string Port { get; set; }

        [TcpCheck(5672)]
        public string Server { get; set; }
    }
}
