using System.Collections.Generic;
using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestHttpCheckListModel
    {
        [HttpCheck("/api/isalive")]
        public string[] Services { get; set; }
        [HttpCheck("/api/isalive")]
        public List<string> List { get; set; }
        [HttpCheck("/api/isalive")]
        public IList<string> IList { get; set; }
        [HttpCheck("/api/isalive")]
        public IReadOnlyList<string> RoList { get; set; }
        [HttpCheck("/api/isalive")]
        public IReadOnlyCollection<string> RoCollection { get; set; }
        [HttpCheck("/api/isalive")]
        public IEnumerable<string> Enumerable { get; set; }
    }
}
