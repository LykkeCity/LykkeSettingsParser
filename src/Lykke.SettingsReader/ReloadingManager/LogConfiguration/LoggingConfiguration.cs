using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Lykke.SettingsReader.ReloadingManager.LogConfiguration
{
    /// <summary>
    /// Microsoft.Extensions.Configuration.IConfiguration implementation for logging configuration
    /// </summary>
    public class LoggingConfiguration<T> : IConfiguration
    {
        private readonly IReloadingManager<T> _manager;
        private readonly LogSettings _logSettings;

        /// <summary>
        /// Gets or sets a configuration value.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <returns>The configuration value.</returns>
        public string this[string key]
        {
            get => GetSection(key).Value;
            set => throw new NotImplementedException();
        }

        internal LoggingConfiguration(IReloadingManager<T> manager, LogSettings logSettings)
        {
            _manager = manager;
            _logSettings = logSettings;
        }

        /// <summary>
        /// Gets the immediate descendant configuration sub-sections.
        /// </summary>
        /// <returns>The configuration sub-sections.</returns>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            if (_logSettings == null)
                return new IConfigurationSection[0];

            var result = new List<IConfigurationSection>
            {
                new LoggingConfigurationField<T>(_manager, nameof(LogSettings.IncludeScopes), null, _logSettings.IncludeScopes.ToString()),
                new LoggingConfigurationSection<T>(_manager, nameof(LogSettings.LogLevel), new LogProviderSettings { LogLevel = _logSettings.LogLevel })
            };
            if (_logSettings.Console != null)
                result.Add(new LoggingConfigurationSection<T>(_manager, nameof(LogSettings.Console), _logSettings.Console));
            if (_logSettings.Debug != null)
                result.Add(new LoggingConfigurationSection<T>(_manager, nameof(LogSettings.Debug), _logSettings.Debug));
            if (_logSettings.EventLog != null)
                result.Add(new LoggingConfigurationSection<T>(_manager, nameof(LogSettings.EventLog), _logSettings.EventLog));
            if (_logSettings.AzureAppServices != null)
                result.Add(new LoggingConfigurationSection<T>(_manager, nameof(LogSettings.AzureAppServices), _logSettings.AzureAppServices));
            if (_logSettings.TraceSource != null)
                result.Add(new LoggingConfigurationSection<T>(_manager, nameof(LogSettings.TraceSource), _logSettings.TraceSource));
            if (_logSettings.EventSource != null)
                result.Add(new LoggingConfigurationSection<T>(_manager, nameof(LogSettings.EventSource), _logSettings.EventSource));

            return result;
        }

        /// <summary>
        /// Returns a Microsoft.Extensions.Primitives.IChangeToken that can be used to observe when this configuration is reloaded.
        /// </summary>
        /// <returns>A Microsoft.Extensions.Primitives.IChangeToken.</returns>
        public IChangeToken GetReloadToken()
        {
            return new SettingsChangeToken<T>(_manager);
        }

        /// <summary>
        /// Gets a configuration sub-section with the specified key.
        /// </summary>
        /// <param name="key">The key of the configuration section.</param>
        /// <returns>The Microsoft.Extensions.Configuration.IConfigurationSection.</returns>
        /// <remarks>This method will never return null. If no matching sub-section is found with the specified key,
        /// an empty Microsoft.Extensions.Configuration.IConfigurationSection will be returned.</remarks>
        public IConfigurationSection GetSection(string key)
        {
            if (_logSettings == null)
                return new LoggingConfigurationField<T>(_manager, key, null, null);

            switch (key)
            {
                case nameof(LogSettings.Console):
                    return new LoggingConfigurationSection<T>(_manager, key, _logSettings.Console);
                case nameof(LogSettings.Debug):
                    return new LoggingConfigurationSection<T>(_manager, key, _logSettings.Debug);
                case nameof(LogSettings.EventLog):
                    return new LoggingConfigurationSection<T>(_manager, key, _logSettings.EventLog);
                case nameof(LogSettings.AzureAppServices):
                    return new LoggingConfigurationSection<T>(_manager, key, _logSettings.AzureAppServices);
                case nameof(LogSettings.TraceSource):
                    return new LoggingConfigurationSection<T>(_manager, key, _logSettings.TraceSource);
                case nameof(LogSettings.EventSource):
                    return new LoggingConfigurationSection<T>(_manager, key, _logSettings.EventSource);
                case nameof(LogSettings.LogLevel):
                    return new LoggingConfigurationSection<T>(_manager, key, new LogProviderSettings { LogLevel = _logSettings.LogLevel });
                case nameof(LogSettings.IncludeScopes):
                    return new LoggingConfigurationField<T>(_manager, key, null, _logSettings.IncludeScopes.ToString());
                default:
                    return new LoggingConfigurationField<T>(_manager, key, null, null);
            }
        }
    }
}
