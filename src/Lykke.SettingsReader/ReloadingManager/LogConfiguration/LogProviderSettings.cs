using System.Collections.Generic;

namespace Lykke.SettingsReader.ReloadingManager.LogConfiguration
{
    /// <summary>
    /// Log settings for log provider
    /// </summary>
    public class LogProviderSettings
    {
        /// <summary>
        /// Log level settings for current provider
        /// </summary>
        public Dictionary<string, string> LogLevel { get; set; }
    }
}
