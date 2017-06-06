using System;

namespace Lykke.SettingsReader.Exceptions
{
    public class SettingsReaderException : Exception
    {
        public SettingsReaderException()
        {
            
        }

        public SettingsReaderException(string message, Exception e) : base(message, e)
        {

        }
    }
}

