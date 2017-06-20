using System;
using System.Net.Http;

namespace Lykke.SettingsReader
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Loads and parse settings from given <param name="settingsUrl"/> if any,
        /// otherwise settings will be loaded from url specified by "SettingsUrl" environment variable
        /// </summary>
        /// <typeparam name="TSettings">Type of setting to load</typeparam>
        /// <param name="httpClient">Http client</param>
        /// <param name="settingsUrl">Settings url</param>
        /// <returns>Loaded and parsed settings</returns>
        public static TSettings LoadSettings<TSettings>(this HttpClient httpClient, string settingsUrl = null)
        {
            settingsUrl = settingsUrl ?? Environment.GetEnvironmentVariable("SettingsUrl");

            if (string.IsNullOrEmpty(settingsUrl))
            {
                throw new Exception("settingsUrl not specified and environment variable 'SettingsUrl' is not defined");
            }

            using (var response = httpClient.GetAsync(settingsUrl).Result)
            {
                var settingsData = response.Content.ReadAsStringAsync().Result;

                return SettingsProcessor.Process<TSettings>(settingsData);
            }
        }
    }
}