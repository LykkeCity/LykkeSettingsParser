using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestEndpoint
    {
        [TcpCheck]
        public string HostPort { get; set; }
    }
}