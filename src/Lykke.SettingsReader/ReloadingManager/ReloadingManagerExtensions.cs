using System;

namespace Lykke.SettingsReader
{
    public static class ReloadingManagerExtensions {
        public static IReloadingManager<TValue> For<TRoot, TValue>(this IReloadingManager<TRoot> rootManager, Func<TRoot, TValue> expr) {
            return new NestedReloadingManager<TRoot, TValue>(rootManager, expr);
        }
    }
}