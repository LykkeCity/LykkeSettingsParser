using System;

using AzureStorage;
using AzureStorage.Tables;

using Common.Log;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.SettingsReader
{
    public class SettingsConfigurator<TSettings> : ISettingsConfigurator<TSettings>
    {
        private readonly Func<TSettings> _loadSettings;
        private Lazy<TSettings> _currentSettings;

        public IServiceCollection Services { get; }

        public TSettings Settings => _currentSettings.Value;

        public SettingsConfigurator(IServiceCollection services, Func<TSettings> loadSettings)
        {
            _loadSettings = loadSettings;
            _currentSettings = new Lazy<TSettings>(_loadSettings);
            Services = services;
        }

        public TValue GetValue<TValue>(Func<TSettings, TValue> selectValue)
        {
            return selectValue(Settings);
        }

        public ISettingsConfigurator<TSettings> AddSettingsObject<TValue>(Func<TSettings, TValue> selectValue) where TValue : class
        {
            Services.AddScoped<TValue>(x => selectValue(Settings));

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
                    () => selectConnectionString(ReloadSettings()),
                    tableName,
                    x.GetService<ILog>(),
                    maxExecutionTimeout
                )
            );

            return this;
        }

        private TSettings ReloadSettings()
        {
            _currentSettings = new Lazy<TSettings>(_loadSettings);
            return _currentSettings.Value;
        }
    }
}