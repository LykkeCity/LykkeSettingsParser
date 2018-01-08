using System.Collections.Generic;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestAmqpCheckDictionaryModel
    {
        public Dictionary<string, RabbitMq> Rabbits { get; set; }
        public IDictionary<string, RabbitMq> IDict { get; set; }
        public IReadOnlyDictionary<string, RabbitMq> RoDict { get; set; }
    }
}
