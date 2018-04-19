namespace Lykke.SettingsReader.Exceptions
{
    /// <summary>
    /// Exception for failed dependencies check.
    /// </summary>
    public class FailedDependenciesException : SettingsReaderException
    {
        /// <summary>Deafult c-tor.</summary>
        public FailedDependenciesException()
            : base("Application dependencies check is failed.")
        {
        }
    }
}