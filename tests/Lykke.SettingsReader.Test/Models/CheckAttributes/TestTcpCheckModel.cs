using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestTcpCheckModel
    {
        [Optional]
        public TestEndpoint HostInfo { get; set; }

        [TcpCheck("Port")]
        [Optional]
        public string Host { get; set; }

        [Optional]
        public string Port { get; set; }

        [TcpCheck(5672)]
        [Optional]
        public string Server { get; set; }
    }
}
