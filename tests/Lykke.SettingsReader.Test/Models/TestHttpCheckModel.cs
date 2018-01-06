using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.Test.Models
{
    public class TestHttpCheckModel
    {
        [HttpCheck("/api/isalive")]
        public string ServiceUrl { get; set; }
        public string Port { get; set; }
        public int Num { get; set; }
    }
}
