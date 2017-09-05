using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

using Lykke.SettingsReader.Exceptions;

namespace Lykke.SettingsReader
{
    public static partial class SettingsProcessor
    {
        private static readonly IDictionary<Type, Func<object, object>> ScalarConverters = new Dictionary<Type, Func<object, object>>
        {
            { typeof(TimeSpan), x => TimeSpan.Parse(x.ToString()) },
            { typeof(Guid), x => Guid.Parse(x.ToString()) },
            { typeof(DateTime), x => DateTime.Parse(x.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind) }
        };

        private static object Convert_FromValue(object value, Type targetType, string path)
        {
            var targetInfo = targetType.GetTypeInfo();

            if (targetInfo.IsGenericType)
            {
                if (targetInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
                    // ReSharper disable once TailRecursiveCall
                    return null == value ? null : Convert_FromValue(value, targetInfo.GenericTypeArguments[0], path);

                throw new PropertyTypeNotSupportedException(path, targetType);
            }

            if (null == value)
            {
                if (targetInfo.IsValueType)
                    throw new PropertyTypeNotAssignableFromNullException(path, targetType);
                return null;
            }

            if (targetInfo.IsEnum)
                return Enum.Parse(targetType, value.ToString(), true);

            return ScalarConverters.ContainsKey(targetType)
                ? ScalarConverters[targetType](value)
                : System.Convert.ChangeType(value, targetType);
        }
    }
}
