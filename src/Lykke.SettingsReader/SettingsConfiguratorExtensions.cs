using Microsoft.Extensions.DependencyInjection;

namespace Lykke.SettingsReader
{
    public static class SettingsConfiguratorExtensions
    {
        public static IReloadingManager<TSettings> LoadSettings<TSettings>(this IServiceCollection services, string settingsUrl) where TSettings : class
        {
            var reloadingManager = new SettingsServiceReloadingManager<TSettings>(settingsUrl);
            services.AddSingleton<IReloadingManager<TSettings>>(reloadingManager);
            return reloadingManager;
        }

        public static IReloadingManager<TSettings> LoadLocalSettings<TSettings>(this IServiceCollection services, string path) where TSettings : class
        {
            var reloadingManager = new LocalSettingsReloadingManager<TSettings>(path);
            services.AddSingleton<IReloadingManager<TSettings>>(reloadingManager);
            return reloadingManager;
        }
    }
}