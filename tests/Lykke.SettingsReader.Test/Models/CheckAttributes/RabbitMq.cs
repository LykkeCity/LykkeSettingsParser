using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class RabbitMq
    {
        [AmqpCheck]
        public string ConnString { get; set; }
    }
}