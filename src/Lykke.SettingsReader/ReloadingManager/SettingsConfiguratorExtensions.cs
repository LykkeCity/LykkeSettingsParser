using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Lykke.SettingsReader
{
    public static class SettingsConfiguratorExtensions
    {
        public static readonly string DefaultConfigurationKey = "SettingsUrl";

        public static IReloadingManagerWithConfiguration<TSettings> LoadSettings<TSettings>(
            this IConfiguration configuration,
            string key = null,
            Action<TSettings> configure = null
        )
            where TSettings : class
        {
            key = key ?? DefaultConfigurationKey;
            var settingsUrl = configuration[key];

            if (string.IsNullOrEmpty(settingsUrl))
            {
                throw new InvalidOperationException($"The connection string to the settings was not found by configuration key '{key}'");
            }

            if (settingsUrl.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                return new SettingsServiceReloadingManager<TSettings>(settingsUrl, configure);
            }

            return new LocalSettingsReloadingManager<TSettings>(settingsUrl);
        }

        public static Task CheckDependenciesAsync<TSettings>(
            this IConfiguration configuration, TSettings settings, string slackConnString, string queueName)
            where TSettings : class
        {
            return SettingsProcessor.CheckDependenciesAsync(settings, slackConnString, queueName);
        }
    }
}
