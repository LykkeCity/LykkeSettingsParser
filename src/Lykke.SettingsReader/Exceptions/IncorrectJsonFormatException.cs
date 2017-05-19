using System;
using System.Reflection.Metadata.Ecma335;

namespace Lykke.SettingsReader.Exceptions
{
    public class IncorrectJsonFormatException : SettingsReaderException
    {
        public IncorrectJsonFormatException(Exception e) : base("Incorrect Json Format", e)
        {
            
        }
    }
}
