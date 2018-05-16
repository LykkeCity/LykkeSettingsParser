using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Lykke.SettingsReader.ReloadingManager.LogConfiguration;

namespace Lykke.SettingsReader
{
    /// <summary>
    /// Extensions for IReloadingManager to provide IConfiguration-like behaviour.
    /// </summary>
    public static class ReloadingManagerConfigurationExtensions
    {
        /// <summary>
        /// Tries to find logging configuration in reloading managers's data
        /// </summary>
        /// <typeparam name="T">Same data type as in provided reloading manager</typeparam>
        /// <param name="manager"></param>
        /// <returns>IConfiguration implementation for logging configuration or null in case such settings section is not found</returns>
        public static IConfiguration GetLoggingConfiguration<T>(this IReloadingManager<T> manager)
        {
            var properties = manager.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var logSettingsProperty = properties.FirstOrDefault(p => p.PropertyType == typeof(LogSettings));
            LogSettings logSettings = logSettingsProperty != null
                ? (LogSettings)logSettingsProperty.GetValue(manager.CurrentValue)
                : null;
            return new LoggingConfiguration<T>(manager, logSettings);
        }
    }
}
