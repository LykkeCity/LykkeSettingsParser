using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Lykke.SettingsReader.Exceptions;

namespace Lykke.SettingsReader
{
    public class SettingsReader
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public static async Task<TSettings> ReadGeneralSettingsLocalAsync<TSettings>(string path)
        {
            using (var reader = File.OpenText(path))
            {
                var content = await reader.ReadToEndAsync();
                return SettingsProcessor.Process<TSettings>(content);
            }
        }

        public static Task<TSettings> ReadGeneralSettingsAsync<TSettings>(string url)
        {
            return new SettingsReader(url).ReadSettingsAsync<TSettings>();
        }

        private readonly ReaderWriterLockSlim _sync = new ReaderWriterLockSlim();
        private Task<string> _currentReadTask;

        private readonly string _settingsUrl;

        public SettingsReader(string settingsUrl)
        {
            if (string.IsNullOrEmpty(settingsUrl))
            {
                throw new SettingsSourceException("SettingsUrl not specified.");
            }

            _settingsUrl = settingsUrl;
        }

        public async Task<TSettings> ReadSettingsAsync<TSettings>()
        {
            var settingsData = await ReadSettingsAsync();
            return SettingsProcessor.Process<TSettings>(settingsData);
        }

        public async Task<TSettings> ReloadSettingsAsync<TSettings>(string currentValue, Func<TSettings, string> selectConnectionString)
        {
            var actualSettings = await ReadSettingsAsync<TSettings>();
            var actualValue = selectConnectionString(actualSettings);

            if (actualValue != currentValue)
            {
                return actualSettings;
            }

            var settingsData = await ReadSettingsAsync(true);
            return SettingsProcessor.Process<TSettings>(settingsData);
        }

        private Task<string> ReadSettingsAsync(bool reload = false)
        {
            try
            {
                _sync.EnterReadLock();

                // Обновление уже запрошенно
                if (_currentReadTask != null && !_currentReadTask.GetAwaiter().IsCompleted)
                {
                    return _currentReadTask;
                }

                // Не требуется обновлять данные
                if (_currentReadTask != null && !reload)
                {
                    return _currentReadTask;
                }
            }
            finally
            {
                _sync.ExitReadLock();
            }

            try
            {
                _sync.EnterWriteLock();

                // double check

                // Обновление уже запрошенно
                if (_currentReadTask != null && !_currentReadTask.GetAwaiter().IsCompleted)
                {
                    return _currentReadTask;
                }

                // Не требуется обновлять данные
                if (_currentReadTask != null && !reload)
                {
                    return _currentReadTask;
                }

                return _currentReadTask = HttpClient.GetStringAsync(_settingsUrl);
            }
            finally
            {
                _sync.ExitWriteLock();
            }
        }
    }
}