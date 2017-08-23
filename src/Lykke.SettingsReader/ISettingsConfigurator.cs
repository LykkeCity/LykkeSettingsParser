using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.SettingsReader
{
    public interface ISettingsConfigurator<out TSettings>
    {
        TValue GetValue<TValue>(Func<TSettings, TValue> selectValue);

        ISettingsConfigurator<TSettings> AddSettingsObject<TValue>(Func<TSettings, TValue> selectValue) where TValue : class;

        ISettingsConfigurator<TSettings> AddTableStorage<TTableEntity>(
            Func<TSettings, string> selectConnectionString,
            string tableName,
            TimeSpan? maxExecutionTimeout = null)

            where TTableEntity : class, ITableEntity, new();
    }
}