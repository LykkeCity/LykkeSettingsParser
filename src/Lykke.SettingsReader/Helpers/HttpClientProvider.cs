using System.Net.Http;

namespace Lykke.SettingsReader.Helpers
{
    internal static class HttpClientProvider
    {
        private static readonly HttpClient Instance = new HttpClient();
        
        static HttpClientProvider()
        {
        }

        public static HttpClient Client => Instance;
    }
}
