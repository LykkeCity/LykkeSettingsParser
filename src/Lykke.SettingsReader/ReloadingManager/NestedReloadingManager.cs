using System;
using System.Threading.Tasks;

namespace Lykke.SettingsReader
{
    public class NestedReloadingManager<TRoot, TValue> : IReloadingManager<TValue> {
        private readonly IReloadingManager<TRoot> _rootManager;
        private readonly Func<TRoot, TValue> _expr;

        public NestedReloadingManager(IReloadingManager<TRoot> rootManager, Func<TRoot, TValue> expr) {
            _rootManager = rootManager;
            _expr = expr;
        }

        public bool HasLoaded => _rootManager.HasLoaded;

        public TValue CurrentValue => _expr(_rootManager.CurrentValue);

        public async Task<TValue> Reload() {
            var value = await _rootManager.Reload();
            return _expr(value);
        }
    }
}