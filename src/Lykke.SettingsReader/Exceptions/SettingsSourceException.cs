using System;

namespace Lykke.SettingsReader.Exceptions
{
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