using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models
{
    public class TestAmqpCheckModel
    {
        [AmqpCheck]
        public string Rabbit { get; set; }
    }
}
