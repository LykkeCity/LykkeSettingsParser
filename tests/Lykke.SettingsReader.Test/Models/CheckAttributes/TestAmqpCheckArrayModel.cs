using System.Collections.Generic;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestAmqpCheckArrayModel
    {
        public RabbitMq[] Rabbits { get; set; }
        public List<RabbitMq> List { get; set; }
        public IList<RabbitMq> IList { get; set; }
        public IReadOnlyList<RabbitMq> RoList { get; set; }
        public IReadOnlyCollection<RabbitMq> RoCollection { get; set; }
        public IEnumerable<RabbitMq> Enumerable { get; set; }
    }
}
