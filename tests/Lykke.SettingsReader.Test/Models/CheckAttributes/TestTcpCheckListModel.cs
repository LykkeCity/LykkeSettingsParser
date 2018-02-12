using System.Collections.Generic;
using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestTcpCheckListModel
    {
        [TcpCheck]
        [Optional]
        public string[] Hosts { get; set; }

        [TcpCheck]
        [Optional]
        public List<string> List { get; set; }

        [TcpCheck]
        [Optional]
        public IList<string> IList { get; set; }

        [TcpCheck]
        [Optional]
        public IReadOnlyList<string> RoList { get; set; }

        [TcpCheck]
        [Optional]
        public IReadOnlyCollection<string> RoCollection { get; set; }

        [TcpCheck]
        [Optional]
        public IEnumerable<string> Enumerable { get; set; }
    }
}
