using System;
using AzureStorage;
using AzureStorage.Tables;

using Common.Log;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.SettingsReader
{
    public static class SettingsConfiguratorExtensions
    {
        public static IReloadingManager<TSettings> LoadSettings<TSettings>(this IServiceCollection services, string settingsUrl) where TSettings : class
        {
            var reloadingManager = new SettingsServiceReloadingManager<TSettings>(settingsUrl);
            services.AddSingleton<IReloadingManager<TSettings>>(reloadingManager);
            return reloadingManager;
        }

        public static IReloadingManager<TSettings> LoadLocalSettings<TSettings>(this IServiceCollection services, string path) where TSettings : class
        {
            var reloadingManager = new LocalSettingsReloadingManager<TSettings>(path);
            services.AddSingleton<IReloadingManager<TSettings>>(reloadingManager);
            return reloadingManager;
        }

        public static INoSQLTableStorage<TTableEntity> GetTableStorage<TTableEntity>(
            this IServiceProvider serviceProvider,
            IReloadingManager<string> connectionStringManager,
            string tableName,
            ILog log = null,
            TimeSpan? maxExecutionTimeout = null)
            where TTableEntity : class, ITableEntity, new()
        {
            log = log ?? serviceProvider.GetService<ILog>();

            string GetConnectionString() => connectionStringManager.Reload().Result;

            return AzureTableStorage<TTableEntity>.Create(GetConnectionString, tableName, log, maxExecutionTimeout);
        }
        
        public static IServiceCollection AddTableStorage<TTableEntity>(
            this IServiceCollection services,
            IReloadingManager<string> connectionStringManager,
            string tableName,
            ILog log = null,
            TimeSpan? maxExecutionTimeout = null)

            where TTableEntity : class, ITableEntity, new()
        {

            services.AddSingleton<INoSQLTableStorage<TTableEntity>>(
                x => x.GetTableStorage<TTableEntity>(connectionStringManager, tableName, log, maxExecutionTimeout)
            );

            return services;
        }
    }
}