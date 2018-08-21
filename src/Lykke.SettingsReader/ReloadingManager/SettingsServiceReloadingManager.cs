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
        private readonly Action<SlackNotificationOptions<TSettings>> _slackNotificationOptions;
        private readonly Action<TSettings> _configure;

        public SettingsServiceReloadingManager(string settingsUrl, Action<SlackNotificationOptions<TSettings>> slackNotificationOptions, Action<TSettings> configure = null)
        {
            if (string.IsNullOrEmpty(settingsUrl))
                throw new ArgumentException("Url not specified.", nameof(settingsUrl));

            _settingsUrl = settingsUrl;
            _slackNotificationOptions = slackNotificationOptions ?? throw new ArgumentNullException(nameof(slackNotificationOptions));
            _configure = configure;
        }

        protected override async Task<TSettings> Load()
        {
            Console.WriteLine($"{DateTime.UtcNow} Reading settings");

            var content = await HttpClientProvider.Client.GetStringAsync(_settingsUrl);
            var processingResult = await SettingsProcessor.ProcessForConfigurationAsync<TSettings>(content);
            var settings = processingResult.Item1;
            SetSettingsConfigurationRoot(processingResult.Item2);

            Task.Run(() => SettingsProcessor.CheckDependenciesAsync(settings, _slackNotificationOptions));
            
            _configure?.Invoke(settings);
            return settings;
        }
    }
}
