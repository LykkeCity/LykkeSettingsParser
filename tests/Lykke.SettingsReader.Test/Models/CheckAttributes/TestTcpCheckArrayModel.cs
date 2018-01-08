using System.Collections.Generic;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestTcpCheckArrayModel
    {
        public TestEndpoint[] Endpoints { get; set; }
        public List<TestEndpoint> List { get; set; }
        public IList<TestEndpoint> IList { get; set; }
        public IReadOnlyList<TestEndpoint> RoList { get; set; }
        public IReadOnlyCollection<TestEndpoint> RoCollection { get; set; }
        public IEnumerable<TestEndpoint> Enumerable { get; set; }
    }
}
