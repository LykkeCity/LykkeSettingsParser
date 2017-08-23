using Microsoft.Extensions.DependencyInjection;

namespace Lykke.SettingsReader
{
    public static class SettingsConfiguratorExtensions
    {
        public static ISettingsConfigurator<TSettings> LoadSettings<TSettings>(this IServiceCollection services, string path) where TSettings : class
        {
            return new SettingsConfigurator<TSettings>(services, path);
        }
    }
}