using System.Collections.Generic;
using Lykke.SettingsReader.Attributes;

namespace Lykke.SettingsReader.ReloadingManager.LogConfiguration
{
    /// <summary>
    /// General logging settings
    /// </summary>
    public class LogSettings
    {
        /// <summary>
        /// Include scopes flad
        /// </summary>
        /// <remarks>Default value = false</remarks>
        [Optional]
        public bool IncludeScopes { get; set; } = false;

        /// <summary>
        /// Default log level settings
        /// </summary>
        public Dictionary<string, string> LogLevel { get; set; }

        /// <summary>
        /// Settings for Console log provider
        /// </summary>
        [Optional]
        public LogProviderSettings Console { get; set; }

        /// <summary>
        /// Settings for Debug log provider
        /// </summary>
        [Optional]
        public LogProviderSettings Debug { get; set; }

        /// <summary>
        /// Settings for EventLog log provider
        /// </summary>
        [Optional]
        public LogProviderSettings EventLog { get; set; }

        /// <summary>
        /// Settings for AzureAppServices log provider
        /// </summary>
        [Optional]
        public LogProviderSettings AzureAppServices { get; set; }

        /// <summary>
        /// Settings for TraceSource log provider
        /// </summary>
        [Optional]
        public LogProviderSettings TraceSource { get; set; }

        /// <summary>
        /// Settings for EventSource log provider
        /// </summary>
        [Optional]
        public LogProviderSettings EventSource { get; set; }
    }
}
