using System;

namespace Lykke.SettingsReader {
    [Obsolete("Will be deleted. Have to use IConfiguration.LoadSettings extension method.")]
    public static class HttpSettingsLoader
    {
        /// <summary>
        /// Loads and parses settings from given <paramref name="settingsUrl"/> if any,
        /// otherwise settings will be loaded from url specified by "SettingsUrl" environment variable
        /// </summary>
        /// <typeparam name="TSettings">Type of setting to load</typeparam>
        /// <param name="settingsUrl">Settings url</param>
        /// <exception cref="SettingsSourceException">
        /// Will be thrown if <paramref name="settingsUrl"/> and environment variable "SettingsUrl"
        /// are both not specified
        /// </exception>
        /// <returns>Loaded and parsed settings</returns>
        public static TSettings Load<TSettings>(string settingsUrl = null)
        {
            settingsUrl = settingsUrl ?? Environment.GetEnvironmentVariable("SettingsUrl");
            if (string.IsNullOrEmpty(settingsUrl))
            {
                throw new SettingsSourceException("settingsUrl not specified and environment variable 'SettingsUrl' is not defined");
            }

            var reloadingManager = new SettingsServiceReloadingManager<TSettings>(settingsUrl);
            return reloadingManager.CurrentValue;
        }
    }

}