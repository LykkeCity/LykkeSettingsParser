using System;
using System.IO;
using System.Threading.Tasks;
using Lykke.SettingsReader.ReloadingManager.Configuration;

namespace Lykke.SettingsReader
{
    public class LocalSettingsReloadingManager<TSettings> : ReloadingManagerWithConfigurationBase<TSettings>
    {
        private readonly string _path;

        public LocalSettingsReloadingManager(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path not specified.", nameof(path));
            }

            _path = path;
        }

        protected override async Task<TSettings> Load()
        {
            using (var reader = File.OpenText(_path))
            {
                var content = await reader.ReadToEndAsync();
                var processingResult = SettingsProcessor.ProcessForConfiguration<TSettings>(content);
                SetSettingsConfigurationRoot(processingResult.Item2);
                return processingResult.Item1;
            }
        }
    }
}