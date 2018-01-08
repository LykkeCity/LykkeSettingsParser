using System.Collections.Generic;
using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestTcpCheckListModel
    {
        [TcpCheck]
        public string[] Hosts { get; set; }
        [TcpCheck]
        public List<string> List { get; set; }
        [TcpCheck]
        public IList<string> IList { get; set; }
        [TcpCheck]
        public IReadOnlyList<string> RoList { get; set; }
        [TcpCheck]
        public IReadOnlyCollection<string> RoCollection { get; set; }
        [TcpCheck]
        public IEnumerable<string> Enumerable { get; set; }
    }
}
