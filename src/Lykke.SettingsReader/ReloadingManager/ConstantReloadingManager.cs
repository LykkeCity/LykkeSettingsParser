using System.Threading.Tasks;

namespace Lykke.SettingsReader.ReloadingManager
{
    public class ConstantReloadingManager<T> : ReloadingManagerBase<T>
    {
        private readonly T _value;

        public ConstantReloadingManager(T value)
        {
            _value = value;
        }

        protected override Task<T> Load()
        {
            return Task.FromResult(_value);
        }
    }
}
