using System;

namespace Lykke.SettingsReader.Exceptions
{
    public class IncorrectJsonFormatException : SettingsReaderException
    {
        public IncorrectJsonFormatException(Exception e) : base("Incorrect Json Format", e)
        {
            
        }
    }
}
