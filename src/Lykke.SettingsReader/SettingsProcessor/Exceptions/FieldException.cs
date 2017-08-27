using System;

namespace Lykke.SettingsReader
{
    public class FieldException : SettingsReaderException
    {
        public string FieldName { get; }

        public FieldException(string fieldName, string message, Exception ex = null)
            : base(message, ex)
        {
            FieldName = fieldName;
        }
    }
}