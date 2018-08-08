using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Lykke.SettingsReader
{
    /// <summary>
    /// Extensions for IConfiguration
    /// </summary>
    public static class SettingsConfiguratorExtensions
    {
        public static readonly string DefaultConfigurationKey = "SettingsUrl";

        /// <summary>
        /// Loads settings
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="key">key name in the configuration</param>
        /// <param name="configure">action to configure settings</param>
        /// <typeparam name="TSettings">model for settings</typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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

        /// <summary>
        /// Checks dependencies
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="settings">settings model to check dependencies</param>
        /// <param name="slackConnString">slack connection string to send message about failed dependencies</param>
        /// <param name="queueName">queue name for slack notifications</param>
        /// <param name="sender">name of the sender in slack message</param>
        /// <typeparam name="TSettings"></typeparam>
        /// <returns></returns>
        public static Task CheckDependenciesAsync<TSettings>(
            this IConfiguration configuration, TSettings settings, string slackConnString, string queueName, string sender)
            where TSettings : class
        {
            if (string.IsNullOrEmpty(slackConnString))
                throw new ArgumentNullException(nameof(slackConnString));
            
            if (string.IsNullOrEmpty(queueName))
                throw new ArgumentNullException(nameof(queueName));
            
            if (string.IsNullOrEmpty(sender))
                throw new ArgumentNullException(nameof(sender));
            
            return SettingsProcessor.CheckDependenciesAsync(settings, slackConnString, queueName, sender);
        }
    }
}
