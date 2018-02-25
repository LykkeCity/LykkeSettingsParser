namespace Lykke.SettingsReader.ReloadingManager
{
    public static class ConstantReloadingManager
    {
        public static IReloadingManager<T> From<T>(T value)
        {
            return new GenericReloadingManager<T>(value);
        }
    }
}
