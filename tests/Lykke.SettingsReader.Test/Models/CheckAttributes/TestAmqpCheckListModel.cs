using System.Collections.Generic;
using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestAmqpCheckListModel
    {
        [AmqpCheck]
        [Optional]
        public string[] Rabbits { get; set; }

        [AmqpCheck]
        [Optional]
        public List<string> List { get; set; }

        [AmqpCheck]
        [Optional]
        public IList<string> IList { get; set; }

        [AmqpCheck]
        [Optional]
        public IReadOnlyList<string> RoList { get; set; }

        [AmqpCheck]
        [Optional]
        public IReadOnlyCollection<string> RoCollection { get; set; }

        [AmqpCheck]
        [Optional]
        public IEnumerable<string> Enumerable { get; set; }
    }
}
