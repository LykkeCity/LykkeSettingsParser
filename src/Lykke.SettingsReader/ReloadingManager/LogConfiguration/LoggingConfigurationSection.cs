using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Lykke.SettingsReader.ReloadingManager.LogConfiguration
{
    /// <summary>
    /// Microsoft.Extensions.Configuration.IConfigurationSection implementation for logging configuration section
    /// </summary>
    public sealed class LoggingConfigurationSection<T> : IConfigurationSection
    {
        private readonly IReloadingManager<T> _manager;
        private readonly LogProviderSettings _providerSettings;

        /// <summary>
        /// Gets the key this section occupies in its parent.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets the full path to this section within the Microsoft.Extensions.Configuration.IConfiguration.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the section value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a configuration value.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <returns>The configuration value.</returns>
        public string this[string key]
        {
            get
            {
                return _providerSettings != null && _providerSettings.LogLevel != null && _providerSettings.LogLevel.ContainsKey(key)
                    ? _providerSettings.LogLevel[key]
                    : null;
            }
            set => throw new NotImplementedException();
        }

        internal LoggingConfigurationSection(
            IReloadingManager<T> manager,
            string key,
            LogProviderSettings providerSettings)
        {
            _manager = manager;
            Key = key;
            Path = key;
            Value = null;
            _providerSettings = providerSettings;
        }

        /// <summary>
        /// Gets the immediate descendant configuration sub-sections.
        /// </summary>
        /// <returns>The configuration sub-sections.</returns>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            if (_providerSettings == null || _providerSettings.LogLevel == null)
                return new IConfigurationSection[0];
            return _providerSettings.LogLevel
                .Select(p => new LoggingConfigurationField<T>(_manager, p.Key, Path, p.Value))
                .ToList();
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
            return new LoggingConfigurationField<T>(
                _manager,
                key,
                Path,
                this[key]);
        }
    }
}
