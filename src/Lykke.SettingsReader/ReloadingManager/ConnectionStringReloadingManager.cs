using System;
using System.Threading.Tasks;

namespace Lykke.SettingsReader
{
    public class ConnectionStringReloadingManager<TSettings> : ReloadingManagerBase<string>
    {
        private readonly IReloadingManager<TSettings> _rootManager;
        private readonly Func<TSettings, string> _selectConnectionString;

        public ConnectionStringReloadingManager(IReloadingManager<TSettings> rootManager, Func<TSettings, string> selectConnectionString)
        {
            _rootManager = rootManager;
            _selectConnectionString = selectConnectionString;
        }

        private string _currentConnectionString;

        protected override async Task<string> Load()
        {
            var actualValue = _selectConnectionString(_rootManager.CurrentValue);

            if (actualValue != _currentConnectionString)
            {
                return _currentConnectionString = actualValue;
            }

            return _currentConnectionString = _selectConnectionString(await _rootManager.Reload());
        }
    }
}