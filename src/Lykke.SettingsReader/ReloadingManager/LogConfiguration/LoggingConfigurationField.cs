using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace Lykke.SettingsReader.ReloadingManager.LogConfiguration
{
    /// <summary>
    /// Microsoft.Extensions.Configuration.IConfigurationSection implementation for logging configuration field
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LoggingConfigurationField<T> : IConfigurationSection
    {
        private readonly IReloadingManager<T> _manager;

        /// <summary>
        /// Gets or sets a configuration value.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <returns>The configuration value.</returns>
        public string this[string key]
        {
            get => null;
            set => throw new NotImplementedException();
        }

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

        internal LoggingConfigurationField(
            IReloadingManager<T> manager,
            string key,
            string parentPath,
            string value)
        {
            _manager = manager;
            Key = key;
            Path = string.IsNullOrEmpty(parentPath) ? key : $"{parentPath}:{key}";
            Value = value;
        }

        /// <summary>
        /// Gets the immediate descendant configuration sub-sections.
        /// </summary>
        /// <returns>The configuration sub-sections.</returns>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new IConfigurationSection[0];
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
        public IConfigurationSection GetSection(string key)
        {
            return new LoggingConfigurationField<T>(_manager, key, Path, null);
        }
    }
}
