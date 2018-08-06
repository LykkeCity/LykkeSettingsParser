using System;

namespace Lykke.SettingsReader.Exceptions
{
    /// <summary>
    /// Exception for failed dependencies check.
    /// </summary>
    public class FailedDependenciesException : SettingsReaderException
    {
        /// <summary>Deafult c-tor.</summary>
        public FailedDependenciesException()
            : base("Application dependencies check failed.")
        {
        }

        /// <summary>С-tor with exception message.</summary>
        public FailedDependenciesException(string message)
            : base($"Application dependencies check failed:{Environment.NewLine}{message}")
        {
        }
    }
}
