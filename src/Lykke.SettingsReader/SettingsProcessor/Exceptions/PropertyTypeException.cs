using System;

namespace Lykke.SettingsReader.Exceptions
{
    public class PropertyTypeException : FieldException
    {
        public Type TargetType { get; }

        public PropertyTypeException(string propertyPath, Type targetType, string brief, Exception ex = null)
            : base(propertyPath, $@"Type ""{targetType.FullName}"" of property ""{propertyPath}"" {brief}.", ex)
        {
            TargetType = targetType;
        }
    }
}
