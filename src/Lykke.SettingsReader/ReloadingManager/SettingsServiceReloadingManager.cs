using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.SettingsReader.Helpers;
using Lykke.SettingsReader.ReloadingManager.Configuration;

namespace Lykke.SettingsReader
{
    [PublicAPI]
    public class SettingsServiceReloadingManager<TSettings> : ReloadingManagerWithConfigurationBase<TSettings>
    {
        private readonly string _settingsUrl;
        private readonly Action<TSettings> _configure;
        private readonly Func<TSettings, (string, string, string)> _slackInfo;

        public SettingsServiceReloadingManager(string settingsUrl, Func<TSettings, (string, string, string)> slackInfo, Action<TSettings> configure = null)
        {
            if (string.IsNullOrEmpty(settingsUrl))
            {
                throw new ArgumentException("Url not specified.", nameof(settingsUrl));
            }

            _settingsUrl = settingsUrl;
            _configure = configure;
            _slackInfo = slackInfo;
        }

        protected override async Task<TSettings> Load()
        {
            Console.WriteLine($"{DateTime.UtcNow} Reading settings");

            var content = await HttpClientProvider.Client.GetStringAsync(_settingsUrl);
            var processingResult = await SettingsProcessor.ProcessForConfigurationAsync<TSettings>(content);
            var settings = processingResult.Item1;
            SetSettingsConfigurationRoot(processingResult.Item2);

            Task.Run(() => SettingsProcessor.CheckDependenciesAsync(settings, _slackInfo));
            
            _configure?.Invoke(settings);
            return settings;
        }
    }
}
