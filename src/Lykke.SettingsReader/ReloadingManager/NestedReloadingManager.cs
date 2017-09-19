using System;
using System.Threading.Tasks;

namespace Lykke.SettingsReader
{
    public class NestedReloadingManager<TRoot, T> : ReloadingManagerBase<T>
    {
        private readonly IReloadingManager<TRoot> _rootManager;
        private readonly Func<TRoot, T> _expr;
        private readonly Func<T, T, bool> _equal;

        private static bool DefaultEqual(T actual, T current)
        {
            switch (actual)
            {
                case IEquatable<T> t:
                    return t.Equals(current);
                default:
                    return Equals(actual, current);
            }
        }

        public NestedReloadingManager(IReloadingManager<TRoot> rootManager, Func<TRoot, T> expr, Func<T, T, bool> equal = null)
        {
            _rootManager = rootManager;
            _expr = expr;
            _equal = equal ?? DefaultEqual;
        }

        private T _current;

        protected override async Task<T> Load()
        {
            var actualValue = _expr(_rootManager.CurrentValue);

            if (_equal != null && !_equal(actualValue, _current))
            {
                return _current = actualValue;
            }

            return _current = _expr(await _rootManager.Reload());
        }
    }
}