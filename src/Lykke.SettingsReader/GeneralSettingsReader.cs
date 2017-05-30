using System;
using System.IO;
using System.Net.Http;

namespace Lykke.SettingsReader
{
    public class SettingsReader
    {
        public static T ReadGeneralSettings<T>(Uri url)
        {
            var httpClient = new HttpClient { BaseAddress = url };
            var settingsData = httpClient.GetStringAsync("").Result;

            return  SettingsProcessor.Process<T>(settingsData);
        }

        public static T ReadGeneralSettings<T>(string path)
        {
            var content = File.ReadAllText(path);

            return SettingsProcessor.Process<T>(content);
        }
    }
}
