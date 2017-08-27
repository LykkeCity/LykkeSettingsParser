using System.Threading.Tasks;

namespace Lykke.SettingsReader
{
    public interface IReloadingManager<T> {
        bool HasLoaded { get; }

        T CurrentValue { get; }

        Task<T> Reload();
    }
}