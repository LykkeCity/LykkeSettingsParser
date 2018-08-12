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
        private readonly Func<TSettings, (string, string, string)> _slackInfo;

        public LocalSettingsReloadingManager(string path, Func<TSettings, (string, string, string)> slackInfo)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path not specified.", nameof(path));
            }

            _path = path;
            _slackInfo = slackInfo;
        }

        protected override async Task<TSettings> Load()
        {
            using (var reader = File.OpenText(_path))
            {
                var content = await reader.ReadToEndAsync();
                var processingResult = await SettingsProcessor.ProcessForConfigurationAsync<TSettings>(content);
                SetSettingsConfigurationRoot(processingResult.Item2);

                Task.Run(() => SettingsProcessor.CheckDependenciesAsync(processingResult.Item1, _slackInfo));
                
                return processingResult.Item1;
            }
        }
    }
}
