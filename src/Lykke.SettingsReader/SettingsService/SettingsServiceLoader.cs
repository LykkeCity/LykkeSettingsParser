using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Lykke.SettingsReader.Exceptions;

namespace Lykke.Extensions.Configuration {
    public class SettingsServiceLoader {
        private readonly string _settingsUrlEnvironmentVariable;

        private readonly HttpClient _httpClient = new HttpClient();
        private readonly ReaderWriterLockSlim _sync = new ReaderWriterLockSlim();
        private Task<string> _currentReadTask = null;

        public SettingsServiceLoader(string settingsUrlEnvironmentVariable = null) {
            _settingsUrlEnvironmentVariable = settingsUrlEnvironmentVariable ?? "SettingsUrl";
        }

        public Task<string> ReadSettingsAsync(bool reload = false, string currentValue = null)
        {
            try
            {
                _sync.EnterReadLock();

                // Обновление уже запрошенно
                if (_currentReadTask != null && !_currentReadTask.IsCompleted)
                {
                    return _currentReadTask;
                }

                // Не требуется обновлять данные
                if (_currentReadTask != null && !reload)
                {
                    return _currentReadTask;
                }

                // Обновленные данные уже полученны
                if (_currentReadTask != null && _currentReadTask.Status == TaskStatus.RanToCompletion && _currentReadTask.Result != currentValue)
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
                if (_currentReadTask != null && !_currentReadTask.IsCompleted)
                {
                    return _currentReadTask;
                }

                // Не требуется обновлять данные
                if (_currentReadTask != null && !reload)
                {
                    return _currentReadTask;
                }

                // Обновленные данные уже полученны
                if (_currentReadTask != null && _currentReadTask.Status == TaskStatus.RanToCompletion && _currentReadTask.Result != currentValue)
                {
                    return _currentReadTask;
                }

                var settingsUrl = Environment.GetEnvironmentVariable(_settingsUrlEnvironmentVariable);

                return _currentReadTask = string.IsNullOrEmpty(settingsUrl)
                    ? Task.FromException<string>(new SettingsSourceException($"Еnvironment variable '{_settingsUrlEnvironmentVariable}' is not defined"))
                    : _httpClient.GetStringAsync(settingsUrl);
            }
            finally
            {
                _sync.ExitWriteLock();
            }
        }
    }
}