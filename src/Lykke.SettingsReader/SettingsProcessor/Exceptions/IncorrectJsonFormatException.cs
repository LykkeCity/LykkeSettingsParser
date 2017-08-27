using System;

namespace Lykke.SettingsReader
{
    public class IncorrectJsonFormatException : SettingsReaderException
    {
        public IncorrectJsonFormatException(Exception e) : base("Incorrect Json Format", e)
        {
            
        }
    }
}
