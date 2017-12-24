using System.Threading.Tasks;

namespace Lykke.SettingsReader.ReloadingManager
{
    internal class GenericReloadingManager<T> : ReloadingManagerBase<T>
    {
        private readonly T _value;

        public GenericReloadingManager(T value)
        {
            _value = value;
        }

        protected override Task<T> Load()
        {
            return Task.FromResult(_value);
        }
    }
}