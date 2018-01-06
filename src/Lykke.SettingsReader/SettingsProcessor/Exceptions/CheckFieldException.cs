using System;
using System.Collections.Generic;
using System.Text;
using Lykke.SettingsReader.Exceptions;

namespace Lykke.SettingsReader.Exceptions
{
    public class CheckFieldException : FieldException
    {
        public CheckFieldException(string fieldName, string message, Exception ex = null)
            : base(fieldName, message, ex)
        {
            
        }
    }
}
