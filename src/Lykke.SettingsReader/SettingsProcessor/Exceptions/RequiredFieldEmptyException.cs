using System;

namespace Lykke.SettingsReader
{
    public class RequiredFieldEmptyException : FieldException
    {
        public RequiredFieldEmptyException(string propertyPath, Exception ex = null)
            : base(propertyPath, $@"The field ""{propertyPath}"" empty in a json file.", ex)
        { }
    }
}
