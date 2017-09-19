using System;

namespace Lykke.SettingsReader.Exceptions
{
    public class RequiredFieldEmptyException : FieldException
    {
        public RequiredFieldEmptyException(string propertyPath, Exception ex = null)
            : base(propertyPath, $@"The field ""{propertyPath}"" empty in provided json.", ex)
        { }
    }
}
