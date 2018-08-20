using System;
using JetBrains.Annotations;

namespace Lykke.SettingsReader.ReloadingManager.Configuration
{
    /// <summary>
    /// Options for configuring slack for failed dependencies checks
    /// </summary>
    [PublicAPI]
    public class SlackNotificationOptions<TAppSettings>
    {
        private readonly TAppSettings _settings;

        /// <summary>
        /// Azure connection string
        /// </summary>
        internal string ConnectionString { get; set; }
        
        /// <summary>
        /// Azure queue name
        /// </summary>
        internal string QueueName { get; set; }
        
        /// <summary>
        /// Sender name used in slack notification message
        /// </summary>
        public string SenderName { get; set; }

        internal SlackNotificationOptions(TAppSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Sets azure connection string 
        /// </summary>
        /// <param name="connStringFunction">function to return azure connection string</param>
        public void SetConnString(Func<TAppSettings, string> connStringFunction)
        {
            ConnectionString = connStringFunction(_settings);
        }
        
        /// <summary>
        /// Sets azure queue name 
        /// </summary>
        /// <param name="queueFunction">function to return azure queue name</param>
        public void SetQueueName(Func<TAppSettings, string> queueFunction)
        {
            QueueName = queueFunction(_settings);
        }
    }
}
