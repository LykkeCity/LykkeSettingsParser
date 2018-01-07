using System;

namespace Lykke.SettingsReader.Exceptions
{
    public class CheckFieldException : FieldException
    {
        public object CurrentValue { get; set; }

        public CheckFieldException(string fieldName, object currentValue, string message, Exception ex = null)
            : base(fieldName, BuildMessage(message, fieldName, currentValue), ex)
        {
            CurrentValue = currentValue;
        }

        private static string BuildMessage(string message, string fieldName, object value)
        {
            return $"Check of the '{fieldName}' field value [{value}] is failed: {message}";
        }
    }
}
