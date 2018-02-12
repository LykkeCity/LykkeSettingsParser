using System;

namespace Lykke.SettingsReader.Exceptions
{
    /// <summary>
    /// Exception for failed field check
    /// </summary>
    public class CheckFieldException : FieldException
    {
        /// <summary>
        /// Field value
        /// </summary>
        public object CurrentValue { get; set; }

        /// <summary>
        /// C-tor for CheckFieldException
        /// </summary>
        /// <param name="fieldName">Field name</param>
        /// <param name="currentValue">Field value</param>
        /// <param name="message">Problem description</param>
        /// <param name="ex">Optional base exception</param>
        public CheckFieldException(
            string fieldName,
            object currentValue,
            string message,
            Exception ex = null)
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
