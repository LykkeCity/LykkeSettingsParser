using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.SettingsReader.ReloadingManager.Configuration;

namespace Lykke.SettingsReader
{
    [PublicAPI]
    public class LocalSettingsReloadingManager<TSettings> : ReloadingManagerWithConfigurationBase<TSettings>
    {
        private readonly string _path;
        private readonly Action<SlackNotificationOptions<TSettings>> _slackNotificationOptions;
        private readonly bool _throwExceptionOnCheckError;

        public LocalSettingsReloadingManager(string path, 
            Action<SlackNotificationOptions<TSettings>> slackNotificationOptions,
            bool throwExceptionOnCheckError = false)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path not specified.", nameof(path));

            _path = path;
            _slackNotificationOptions = slackNotificationOptions;
            _throwExceptionOnCheckError = throwExceptionOnCheckError;
        }

        protected override async Task<TSettings> Load()
        {
            using (var reader = File.OpenText(_path))
            {
                var content = await reader.ReadToEndAsync();
                var processingResult = await SettingsProcessor.ProcessForConfigurationAsync<TSettings>(content);
                SetSettingsConfigurationRoot(processingResult.Item2);

                if (!_throwExceptionOnCheckError)
                {
                    Task.Run(() => SettingsProcessor.CheckDependenciesAsync(processingResult.Item1, _slackNotificationOptions));
                }
                else
                {
                    var errorMessages = await SettingsProcessor.CheckDependenciesAsync(processingResult.Item1,
                        _slackNotificationOptions);

                    if (!string.IsNullOrEmpty(errorMessages))
                    {
                        throw new Exception($"Services check failed:{Environment.NewLine}{errorMessages} ");
                    }
                }
                
                
                return processingResult.Item1;
            }
        }
    }
}
