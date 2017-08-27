using System;

namespace Lykke.SettingsReader
{
    public class SettingsReaderException : Exception
    {
        public SettingsReaderException()
        {
            
        }

        public SettingsReaderException(string message) : base(message)
        {
        }

        public SettingsReaderException(string message, Exception e) : base(message, e)
        {

        }
    }
}