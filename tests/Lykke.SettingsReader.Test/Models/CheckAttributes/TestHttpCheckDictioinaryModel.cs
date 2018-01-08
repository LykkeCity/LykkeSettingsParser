using System.Collections.Generic;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestHttpCheckDictioinaryModel
    {
        public Dictionary<string, ServiceSettings> Services { get; set; }
        public IDictionary<string, ServiceSettings> IDict { get; set; }
        public IReadOnlyDictionary<string, ServiceSettings> RoDict { get; set; }
    }
}
