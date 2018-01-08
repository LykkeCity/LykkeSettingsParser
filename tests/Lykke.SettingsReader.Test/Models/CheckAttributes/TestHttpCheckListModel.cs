using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models.CheckAttributes
{
    public class TestHttpCheckListModel
    {
        [HttpCheck("/api/isalive")]
        public string[] Services { get; set; }
    }
}