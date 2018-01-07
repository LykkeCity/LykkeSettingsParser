using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models
{
    public class TestAmqpCheckModel
    {
        [AmqpCheck]
        public string ConnStr { get; set; }
        public RabbitMq Rabbit { get; set; }
    }

    public class RabbitMq
    {
        [AmqpCheck]
        public string ConnString { get; set; }
    }
}
