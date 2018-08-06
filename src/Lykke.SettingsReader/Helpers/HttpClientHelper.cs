using System.Net.Http;

namespace Lykke.SettingsReader.Helpers
{
    internal static class HttpClientHelper
    {
        private static readonly HttpClient Instance = new HttpClient();
        
        static HttpClientHelper()
        {
        }

        public static HttpClient Client => Instance;
    }
}
