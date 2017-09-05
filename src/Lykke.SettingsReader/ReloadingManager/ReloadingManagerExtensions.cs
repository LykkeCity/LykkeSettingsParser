using System;

namespace Lykke.SettingsReader
{
    public static class ReloadingManagerExtensions {

        public static IReloadingManager<T> Nested<TRoot, T>(this IReloadingManager<TRoot> rootManager, Func<TRoot, T> expr)
        {
            return new NestedReloadingManager<TRoot, T>(rootManager, expr);
        }

        public static IReloadingManager<string> ConnectionString<TRoot>(this IReloadingManager<TRoot> rootManager, Func<TRoot, string> expr)
        {
            return new NestedReloadingManager<TRoot, string>(rootManager, expr);
        }
    }
}