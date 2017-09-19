using System;

using Lykke.SettingsReader.Exceptions;

namespace Lykke.SettingsReader {
    [Obsolete("Will be deleted. Have to use ArgumentException.")]
    public class SettingsSourceException : SettingsReaderException
    {
        public SettingsSourceException()
        {
        }

        public SettingsSourceException(string message) :
            base(message)
        {
        }

        public SettingsSourceException(string message, Exception inner) :
            base(message, inner)
        {
        }
    }
}