using System;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using JetBrains.Annotations;

namespace Lykke.SettingsReader.ReloadingManager.Configuration
{
    /// <summary>
    /// Base abstract implementation for IReloadingManagerWithConfiguration
    /// </summary>
    /// <typeparam name="T">Type of data to be loaded/reloaded</typeparam>
    [PublicAPI]
    public abstract class ReloadingManagerWithConfigurationBase<T> : ReloadingManagerBase<T>, IReloadingManagerWithConfiguration<T>
    {
        private JToken _token;

        /// <summary>
        /// Property that contains Microsoft.Extensions.Configuration.IConfiguration implementation for IReloadingManager
        /// </summary>
        [NotNull]
        public IConfiguration SettingsConfiguration
        {
            get
            {
                if (!HasLoaded)
                    CurrentTask.GetAwaiter().GetResult();

                if (_token == null)
                    throw new InvalidOperationException("Settings configuration root must be set");

                return new SettingsConfigurationSection<T>(
                    this,
                    null,
                    null,
                    _token);
            }
        }

        /// <summary>
        /// Sets JToken object as configuration root
        /// </summary>
        /// <param name="token">JToken object for settings configuration</param>
        protected void SetSettingsConfigurationRoot(JToken token)
        {
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }
    }
}
