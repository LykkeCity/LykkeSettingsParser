using System;

namespace Lykke.SettingsReader
{
    public static class ReloadingManagerExtensions {
        public static IReloadingManager<string> ConnectionString<TRoot>(this IReloadingManager<TRoot> rootManager, Func<TRoot, string> expr)
        {
            return new ConnectionStringReloadingManager<TRoot>(rootManager, expr);
        }
    }
}