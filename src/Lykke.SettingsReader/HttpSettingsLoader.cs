using System;
using System.Net.Http;

namespace Lykke.SettingsReader
{
    public static class HttpSettingsLoader
    {
        public static TSettings Load<TSettings>(string settingsUrl = null)
        {
            settingsUrl = settingsUrl ?? Environment.GetEnvironmentVariable("SettingsUrl");

            if (string.IsNullOrEmpty(settingsUrl))
            {
                throw new Exception("settingsUrl not specified and environment variable 'SettingsUrl' is not defined");
            }

            using (var httpClient = new HttpClient())
            {
                using (var response = httpClient.GetAsync(settingsUrl).Result)
                {
                    var settingsData = response.Content.ReadAsStringAsync().Result;

                    return SettingsProcessor.Process<TSettings>(settingsData);
                }
            }
        }
    }
}