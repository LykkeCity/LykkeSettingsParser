using System.Net.Http;
using Lykke.SettingsReader.Exceptions;

namespace Lykke.SettingsReader
{
    public static class HttpSettingsLoader
    {
        /// <summary>
        /// Loads and parses settings from given <paramref name="settingsUrl"/> if any,
        /// otherwise settings will be loaded from url specified by "SettingsUrl" environment variable
        /// </summary>
        /// <typeparam name="TSettings">Type of setting to load</typeparam>
        /// <param name="settingsUrl">Settings url</param>
        /// <exception cref="SettingsSourceException">
        /// Will be thrown if <paramref name="settingsUrl"/> and environment variable "SettingsUrl"
        /// are both not specified
        /// </exception>
        /// <returns>Loaded and parsed settings</returns>
        public static TSettings Load<TSettings>(string settingsUrl = null)
        {
            using (var httpClient = new HttpClient())
            {
                return httpClient.LoadSettings<TSettings>(settingsUrl);
            }
        }
    }
}