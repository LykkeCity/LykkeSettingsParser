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
        public IServiceCollection Services { get; }

#if DEBUG
        private readonly string _path;

        Task<TSettings> GetSettings() => SettingsReader.ReadGeneralSettingsLocalAsync<TSettings>(_path);
        Task<TSettings> ReloadSettings(string currentValue, Func<TSettings, string> selectConnectionString) 
            => GetSettings();
#else
        private readonly SettingsReader _settingsReader;

        Task<TSettings> GetSettings() => _settingsReader.ReadSettingsAsync<TSettings>();
        Task<TSettings> ReloadSettings(string currentValue, Func<TSettings, string> selectConnectionString) 
            => _settingsReader.ReloadSettingsAsync<TSettings>(currentValue, selectConnectionString);
#endif

        public SettingsConfigurator(IServiceCollection services, string path)
        {
#if DEBUG
            _path = path;
#else
            _settingsReader = new SettingsReader(path);
#endif
            Services = services;
        }

        public TValue GetValue<TValue>(Func<TSettings, TValue> selectValue)
        {
            return selectValue(GetSettings().Result);
        }

        public ISettingsConfigurator<TSettings> AddSettingsValue<TValue>(Func<TSettings, TValue> selectValue) where TValue : class
        {
            Services.AddScoped<TValue>(x => GetValue(selectValue));

            return this;
        }

        public INoSQLTableStorage<TTableEntity> GetTableStorage<TTableEntity>(
            Func<TSettings, string> selectConnectionString,
            string tableName,
            ILog log,
            TimeSpan? maxExecutionTimeout = null)

            where TTableEntity : class, ITableEntity, new() {

            string currentValue = null;
            string GetConnectionString() => currentValue = selectConnectionString(ReloadSettings(currentValue, selectConnectionString).Result);

            return AzureTableStorage<TTableEntity>.Create(GetConnectionString, tableName, log, maxExecutionTimeout);
        }

        public ISettingsConfigurator<TSettings> AddTableStorage<TTableEntity>(
            Func<TSettings, string> selectConnectionString,
            string tableName,
            ILog log = null,
            TimeSpan? maxExecutionTimeout = null)

            where TTableEntity : class, ITableEntity, new()
        {
            Services.AddSingleton<INoSQLTableStorage<TTableEntity>>(
                x => GetTableStorage<TTableEntity>(selectConnectionString, tableName, log ?? x.GetService<ILog>(), maxExecutionTimeout)
            );

            return this;
        }
    }
}