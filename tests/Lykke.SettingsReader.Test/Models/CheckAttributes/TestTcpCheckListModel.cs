using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestTcpCheckListModel
    {
        [TcpCheck]
        public string[] Hosts { get; set; }
    }
}
