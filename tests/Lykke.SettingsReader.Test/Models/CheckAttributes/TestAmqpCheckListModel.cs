using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestAmqpCheckListModel
    {
        [AmqpCheck]
        public string[] Rabbits { get; set; }
    }
}
