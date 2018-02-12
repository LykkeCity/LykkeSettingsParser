using System.Collections.Generic;
using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestTcpCheckArrayModel
    {
        [Optional]
        public TestEndpoint[] Endpoints { get; set; }
        [Optional]
        public List<TestEndpoint> List { get; set; }
        [Optional]
        public IList<TestEndpoint> IList { get; set; }
        [Optional]
        public IReadOnlyList<TestEndpoint> RoList { get; set; }
        [Optional]
        public IReadOnlyCollection<TestEndpoint> RoCollection { get; set; }
        [Optional]
        public IEnumerable<TestEndpoint> Enumerable { get; set; }
    }
}
