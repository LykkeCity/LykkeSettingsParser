using Microsoft.Extensions.Configuration;

namespace Lykke.SettingsReader
{
    public static class SettingsConfiguratorExtensions
    {
        public static readonly string DefaultConfigurationKey = "SettingsUrl";

        public static IReloadingManager<TSettings> LoadSettings<TSettings>(this IConfiguration configuration, string key = null) where TSettings : class
        {
            var settingsUrl = configuration[key ?? DefaultConfigurationKey];
            return new SettingsServiceReloadingManager<TSettings>(settingsUrl);
        }

        public static IReloadingManager<TSettings> LoadLocalSettings<TSettings>(this IConfiguration configuration, string key = null) where TSettings : class
        {
            var localPath = configuration[key ?? DefaultConfigurationKey];
            return new LocalSettingsReloadingManager<TSettings>(localPath);
        }
    }
}