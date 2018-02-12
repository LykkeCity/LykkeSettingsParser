using System.Collections.Generic;
using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestAmqpCheckArrayModel
    {
        [Optional]
        public RabbitMq[] Rabbits { get; set; }
        [Optional]
        public List<RabbitMq> List { get; set; }
        [Optional]
        public IList<RabbitMq> IList { get; set; }
        [Optional]
        public IReadOnlyList<RabbitMq> RoList { get; set; }
        [Optional]
        public IReadOnlyCollection<RabbitMq> RoCollection { get; set; }
        [Optional]
        public IEnumerable<RabbitMq> Enumerable { get; set; }
    }
}
