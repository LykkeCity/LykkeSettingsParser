using System;

namespace Lykke.SettingsReader.Exceptions
{
    public class SettingsLoaderException : Exception
    {
        public SettingsLoaderException()
        {
        }

        public SettingsLoaderException(string message) :
            base(message)
        {
        }

        public SettingsLoaderException(string message, Exception inner) :
            base(message, inner)
        {
        }
    }
}