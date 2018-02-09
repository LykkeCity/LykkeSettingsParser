using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lykke.SettingsReader
{
    public class SettingsServiceReloadingManager<TSettings> : ReloadingManagerBase<TSettings>
    {
        private readonly string _settingsUrl;
        private readonly Action<TSettings> _configure;

        public SettingsServiceReloadingManager(string settingsUrl, Action<TSettings> configure = null)
        {
            if (string.IsNullOrEmpty(settingsUrl))
            {
                throw new ArgumentException("Url not specified.", nameof(settingsUrl));
            }

            _settingsUrl = settingsUrl;
            _configure = configure;
        }

        protected override async Task<TSettings> Load()
        {
            Console.WriteLine($"{DateTime.UtcNow} Reading settings");

            using (var httpClient = new HttpClient())
            {
                var content = await httpClient.GetStringAsync(_settingsUrl);
                var settings = SettingsProcessor.Process<TSettings>(content);
                _configure?.Invoke(settings);
                return settings;
            }
        }
    }
}