using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models
{
    public class WrongTestTcpCheckModel
    {
        [TcpCheck("ServicePort")]
        public string Host { get; set; }

        public string Port { get; set; }
    }
}
