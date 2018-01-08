using System.Collections.Generic;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestTcpCheckDictionaryModel
    {
        public Dictionary<string, TestEndpoint> Endpoints { get; set; }
        public IDictionary<string, TestEndpoint> IDict { get; set; }
        public IReadOnlyDictionary<string, TestEndpoint> RoDict { get; set; }
    }
}
