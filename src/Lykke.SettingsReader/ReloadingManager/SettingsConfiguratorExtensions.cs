﻿using System;
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
        [Obsolete("Use LoadSettings method with Func")]
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
                return new SettingsServiceReloadingManager<TSettings>(settingsUrl, null, configure);
            }

            return new LocalSettingsReloadingManager<TSettings>(settingsUrl, null);
        }

        /// <summary>
        /// Loads settings
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="slackCheckerSettings">function to return connection string, queue name and sender name for slack notifications</param>
        /// <param name="key">key name in the configuration</param>
        /// <param name="configure">action to configure settings</param>
        /// <typeparam name="TSettings">model for settings</typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IReloadingManagerWithConfiguration<TSettings> LoadSettings<TSettings>(
            this IConfiguration configuration,
            Func<TSettings, (string slackConnString, string queueName, string senderName)> slackCheckerSettings,
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
            
            if (slackCheckerSettings == null)
                throw new ArgumentNullException(nameof(slackCheckerSettings));

            if (settingsUrl.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                return new SettingsServiceReloadingManager<TSettings>(settingsUrl, slackCheckerSettings, configure);
            }

            return new LocalSettingsReloadingManager<TSettings>(settingsUrl, slackCheckerSettings);
        }
    }
}
