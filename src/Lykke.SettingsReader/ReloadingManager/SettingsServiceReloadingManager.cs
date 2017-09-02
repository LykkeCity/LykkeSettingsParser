using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lykke.SettingsReader
{
    public class SettingsServiceReloadingManager<TSettings> : ReloadingManagerBase<TSettings>
    {
        private readonly string _settingsUrl;

        public SettingsServiceReloadingManager(string settingsUrl)
        {
            if (string.IsNullOrEmpty(settingsUrl))
            {
                throw new ArgumentException("Url not specified.", nameof(settingsUrl));
            }

            _settingsUrl = settingsUrl;
        }

        protected override async Task<TSettings> Load()
        {
            using (var httpClient = new HttpClient())
            {
                var content = await httpClient.GetStringAsync(_settingsUrl);
                return SettingsProcessor.Process<TSettings>(content);
            }
        }
    }
}