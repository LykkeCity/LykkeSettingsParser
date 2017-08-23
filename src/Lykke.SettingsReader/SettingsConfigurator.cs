using System;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;

using Common.Log;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.SettingsReader
{
    public class SettingsConfigurator<TSettings> : ISettingsConfigurator<TSettings>
    {
        private readonly string _path;

        public IServiceCollection Services { get; }

#if DEBUG
        Task<TSettings> GetSettings() => SettingsReader.ReadGeneralSettingsLocalAsync<TSettings>(_path);
        Task<TSettings> ReloadSettings(string currentValue, Func<TSettings, string> selectConnectionString) 
            => GetSettings();
#else
        Task<TSettings> GetSettings() => new SettingsReader(_path).ReadSettingsAsync<TSettings>();
        Task<TSettings> ReloadSettings(string currentValue, Func<TSettings, string> selectConnectionString) 
            => new SettingsReader(_path).ReloadSettingsAsync<TSettings>(currentValue, selectConnectionString);
#endif

        public SettingsConfigurator(IServiceCollection services, string path)
        {
            _path = path;
            Services = services;
        }

        public TValue GetValue<TValue>(Func<TSettings, TValue> selectValue)
        {
            return selectValue(GetSettings().Result);
        }

        public ISettingsConfigurator<TSettings> AddSettingsObject<TValue>(Func<TSettings, TValue> selectValue) where TValue : class
        {
            Services.AddScoped<TValue>(x => selectValue(GetSettings().Result));

            return this;
        }

        public ISettingsConfigurator<TSettings> AddTableStorage<TTableEntity>(
            Func<TSettings, string> selectConnectionString,
            string tableName,
            TimeSpan? maxExecutionTimeout = null)

            where TTableEntity : class, ITableEntity, new()
        {
            Services.AddSingleton<INoSQLTableStorage<TTableEntity>>(
                x => AzureTableStorage<TTableEntity>.Create(
                    (currentValue) => selectConnectionString(ReloadSettings(currentValue, selectConnectionString).Result),
                    tableName,
                    x.GetService<ILog>(),
                    maxExecutionTimeout
                )
            );

            return this;
        }
    }
}