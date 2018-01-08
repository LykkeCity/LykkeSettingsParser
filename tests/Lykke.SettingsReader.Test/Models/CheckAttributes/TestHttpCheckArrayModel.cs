using System.Collections.Generic;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestHttpCheckArrayModel
    {
        public ServiceSettings[] Services { get; set; }
        public List<ServiceSettings> List { get; set; }
        public IList<ServiceSettings> IList { get; set; }
        public IReadOnlyList<ServiceSettings> RoList { get; set; }
        public IReadOnlyCollection<ServiceSettings> RoCollection { get; set; }
        public IEnumerable<ServiceSettings> Enumerable { get; set; }
    }
}
