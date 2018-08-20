using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Lykke.SettingsReader.ReloadingManager.Configuration;
using Lykke.SettingsReader.Test.Models;

namespace Lykke.SettingsReader.Test
{
    internal class ReloadingManagerForConfigurations : ReloadingManagerWithConfigurationBase<ConfigurationModel>
    {
        private readonly string _settingsJson;

        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            NullValueHandling = NullValueHandling.Ignore,
        };

        internal ReloadingManagerForConfigurations(ConfigurationModel model, bool ignoreDefaults)
        {
            using (var writer = new StringWriter())
            {
                _serializerSettings.DefaultValueHandling = ignoreDefaults ? DefaultValueHandling.Ignore : DefaultValueHandling.Populate;
                var serializer = JsonSerializer.Create(_serializerSettings);
                serializer.Serialize(writer, model);
                _settingsJson = writer.ToString();
            }
        }

        protected override async Task<ConfigurationModel> Load()
        {
            var processingResult = await SettingsProcessor.ProcessForConfigurationAsync<ConfigurationModel>(_settingsJson);
            var settings = processingResult.Item1;
            SetSettingsConfigurationRoot(processingResult.Item2);
            return settings;
        }
    }
}
