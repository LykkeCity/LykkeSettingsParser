using System;

using AzureStorage;

using Common.Log;

using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.SettingsReader
{
    public interface ISettingsConfigurator<out TSettings> {
        TValue GetValue<TValue>(Func<TSettings, TValue> selectValue);

        ISettingsConfigurator<TSettings> AddSettingsValue<TValue>(Func<TSettings, TValue> selectValue) where TValue: class;

        INoSQLTableStorage<TTableEntity> GetTableStorage<TTableEntity>(
            Func<TSettings, string> selectConnectionString,
            string tableName,
            ILog log,
            TimeSpan? maxExecutionTimeout = null)

            where TTableEntity: class, ITableEntity, new();

        ISettingsConfigurator<TSettings> AddTableStorage<TTableEntity>(
            Func<TSettings, string> selectConnectionString,
            string tableName,
            ILog log = null,
            TimeSpan? maxExecutionTimeout = null)

            where TTableEntity: class, ITableEntity, new();
    }
}