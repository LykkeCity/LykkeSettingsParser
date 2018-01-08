using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestAmqpCheckModel
    {
        [AmqpCheck]
        public string ConnStr { get; set; }
        public RabbitMq Rabbit { get; set; }
    }
}
