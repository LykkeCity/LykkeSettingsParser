using System.Collections.Generic;
using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestAmqpCheckListModel
    {
        [AmqpCheck]
        public string[] Rabbits { get; set; }
        [AmqpCheck]
        public List<string> List { get; set; }
        [AmqpCheck]
        public IList<string> IList { get; set; }
        [AmqpCheck]
        public IReadOnlyList<string> RoList { get; set; }
        [AmqpCheck]
        public IReadOnlyCollection<string> RoCollection { get; set; }
        [AmqpCheck]
        public IEnumerable<string> Enumerable { get; set; }
    }
}
