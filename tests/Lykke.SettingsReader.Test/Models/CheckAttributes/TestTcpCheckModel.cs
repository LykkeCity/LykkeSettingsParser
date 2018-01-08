using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestTcpCheckModel
    {
        public TestEndpoint HostInfo { get; set; }
        [TcpCheck("Port")]
        public string Host { get; set; }

        public string Port { get; set; }

        [TcpCheck(5672)]
        public string Server { get; set; }
    }
}
