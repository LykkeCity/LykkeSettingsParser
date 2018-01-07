using System;

namespace Lykke.SettingsReader.Exceptions
{
    public class CheckFieldException : FieldException
    {
        public object CurrentValue { get; set; }

        public CheckFieldException(string fieldName, object currentValue, string message, Exception ex = null)
            : base(fieldName, message, ex)
        {
            CurrentValue = currentValue;
        }
    }
}
