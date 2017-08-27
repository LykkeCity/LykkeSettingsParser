using System;

namespace Lykke.SettingsReader
{
    public class PropertyTypeNotSupportedException : PropertyTypeException
    {
        public PropertyTypeNotSupportedException(string propertyPath, Type targetType, Exception ex = null)
            : base(propertyPath, targetType, (string) "is not supported", ex)
        { }
    }
}