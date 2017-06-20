using System.Net.Http;

namespace Lykke.SettingsReader
{
    public static class HttpSettingsLoader
    {
        /// <summary>
        /// Loads and parse settings from given <param name="settingsUrl"/> if any,
        /// otherwise settings will be loaded from url specified by "SettingsUrl" environment variable
        /// </summary>
        /// <typeparam name="TSettings">Type of setting to load</typeparam>
        /// <param name="settingsUrl">Settings url</param>
        /// <returns>Loaded and parsed settings</returns>
        public static TSettings Load<TSettings>(string settingsUrl = null)
        {
            using (var httpClient = new HttpClient())
            {
                return httpClient.LoadSettings<TSettings>();
            }
        }
    }
}