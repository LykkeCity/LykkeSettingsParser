using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lykke.SettingsReader
{
    public class SettingsServiceReloadingManager<TSettings> : ReloadingManagerBase<TSettings>
    {
        private readonly string _settingsUrl;
        private readonly Action<TSettings> _configure;
        private readonly bool _disableDependenciesCheck;

        public SettingsServiceReloadingManager(string settingsUrl, Action<TSettings> configure = null)
        {
            if (string.IsNullOrEmpty(settingsUrl))
            {
                throw new ArgumentException("Url not specified.", nameof(settingsUrl));
            }

            _settingsUrl = settingsUrl;
            _configure = configure;
            _disableDependenciesCheck = false;
        }

        public SettingsServiceReloadingManager(string settingsUrl, bool disableDependenciesCheck)
        {
            if (string.IsNullOrEmpty(settingsUrl))
                throw new ArgumentException("Url not specified.", nameof(settingsUrl));

            _settingsUrl = settingsUrl;
            _configure = null;
            _disableDependenciesCheck = disableDependenciesCheck;
        }

        protected override async Task<TSettings> Load()
        {
            Console.WriteLine($"{DateTime.UtcNow} Reading settings");

            using (var httpClient = new HttpClient())
            {
                var content = await httpClient.GetStringAsync(_settingsUrl);
                var settings = SettingsProcessor.Process<TSettings>(content, _disableDependenciesCheck);
                _configure?.Invoke(settings);
                return settings;
            }
        }
    }
}