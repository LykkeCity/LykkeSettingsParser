using Microsoft.Extensions.Configuration;

namespace Lykke.SettingsReader
{
    /// <summary>
    /// Interface for IReloadingManager that provides Microsoft.Extensions.Configuration.IConfiguration functionality
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReloadingManagerWithConfiguration<T> : IReloadingManager<T>
    {
        /// <summary>
        /// Property that contains Microsoft.Extensions.Configuration.IConfiguration implementation for IReloadingManager
        /// </summary>
        IConfiguration SettingsConfiguration { get; }
    }
}
