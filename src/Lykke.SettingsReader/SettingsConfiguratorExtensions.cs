using Microsoft.Extensions.DependencyInjection;

namespace Lykke.SettingsReader
{
    public static class SettingsConfiguratorExtensions
    {
        public static ISettingsConfigurator<TSettings> LoadSettings<TSettings>(this IServiceCollection services, string path) where TSettings : class
        {
#if DEBUG
            TSettings GetSettings() => SettingsReader.ReadGeneralSettingsLocal<TSettings>(path);
#else
            TSettings GetSettings() => HttpSettingsLoader.Load<TSettings>(path);
#endif
            return new SettingsConfigurator<TSettings>(services, GetSettings);
        }
    }
}