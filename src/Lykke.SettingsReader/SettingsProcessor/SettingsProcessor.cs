using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Lykke.SettingsReader.Attributes;
using Lykke.SettingsReader.Exceptions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lykke.SettingsReader
{
    /// <summary>
    /// Class for settings json parsing and validation.
    /// </summary>
    public static partial class SettingsProcessor
    {
        /// <summary>
        /// Parses and validates settings json.
        /// </summary>
        /// <typeparam name="T">Type for parsing</typeparam>
        /// <param name="json">Input json</param>
        /// <returns>Parsed object of generic type T</returns>
        public static T Process<T>(string json)
        {
            return Process<T>(json, false);
        }

        /// <summary>
        /// Parses and validates settings json.
        /// </summary>
        /// <typeparam name="T">Type for parsing</typeparam>
        /// <param name="json">Input json</param>
        /// <param name="disableDependenciesCheck">Flag that can disable dependencies check</param>
        /// <returns>Parsed object of generic type T</returns>
        public static T Process<T>(string json, bool disableDependenciesCheck)
        {
            if (string.IsNullOrEmpty(json))
                throw new JsonStringEmptyException();

            JToken jsonObj;
            try
            {
                jsonObj = (JToken)JsonConvert.DeserializeObject(json);
            }
            catch (Exception e)
            {
                throw new IncorrectJsonFormatException(e);
            }

            var result = FillChildrenFields<T>(jsonObj);

            if (!disableDependenciesCheck)
            {
                Console.WriteLine("Checking services");
                string errorMessage = ProcessChecks(result);
                if (errorMessage != null)
                    throw new FailedDependenciesException(errorMessage);
                Console.WriteLine("Checking services - Done.");
            }

            return result;
        }

        private static T FillChildrenFields<T>(JToken jsonObj, string path = "")
        {
            return (T)Convert(jsonObj, typeof(T), path);
        }

        private static object Convert(JToken jsonObj, Type targetType, string path)
        {
            switch (jsonObj.Type)
            {
                case JTokenType.Object:
                    return Convert_FromObject((JObject)jsonObj, targetType, path);
                case JTokenType.Array:
                    return Convert_FromArray((JArray)jsonObj, targetType, path);
                case JTokenType.Property:
                    return Convert_FromProperty((JProperty)jsonObj, targetType, path);
                default:
                    return Convert_FromValue(((JValue)jsonObj).Value, targetType, path);
            }
        }

        private static object Convert_FromArray(JArray jsonObj, Type targetType, string path)
        {
            if (!IsEnumerable(targetType))
            {
                throw new RequiredFieldEmptyException($"{path}.{jsonObj}".Trim('.'));
            }
            var childType = targetType.IsArray
                ? targetType.GetElementType()
                : targetType.GenericTypeArguments.First();

            var concreteType = typeof(List<>).MakeGenericType(childType);
            var res = (IList)Activator.CreateInstance(concreteType);

            foreach (var elem in jsonObj)
            {
                var propertyPath = ConcatPath(path, res.Count.ToString());
                res.Add(Convert(elem, childType, propertyPath));
            }

            if (targetType.IsArray)
            {
                var arr = Array.CreateInstance(targetType.GetElementType(), res.Count);
                for (var ii = 0; ii < res.Count; ii++)
                {
                    arr.SetValue(res[ii], ii);
                }
                return arr;

            }
            else
            {
                return res;
            }
        }

        private static bool IsGenericEnumerable(Type type)
        {
            return type.GetTypeInfo().IsGenericType &&
                type.GetTypeInfo().GetInterfaces().Any(
                ti => (ti == typeof(IEnumerable<>) || ti.Name == "IEnumerable"));
        }

        private static bool IsEnumerable(Type type)
        {
            return IsGenericEnumerable(type) || type.IsArray;
        }

        private static object Convert_FromProperty(JProperty jsonObj, Type targetType, string path)
        {
            return Convert(jsonObj.Value, targetType, path);
        }

        private static string ConcatPath(string path, string propertyName)
        {
            return $"{path}.{propertyName}".Trim('.');
        }

        private static string ProcessChecks<T>(T model)
        {
            if (model == null)
                return null;

            Type objType = model.GetType();
            PropertyInfo[] properties = objType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !p.GetIndexParameters().Any())
                .ToArray();

            var errorMessages = new List<string>();
            Parallel.ForEach(properties, p =>
            {
                string errorMessage = CheckProperty(p, model);
                if (errorMessage == null)
                    return;

                lock(errorMessages)
                {
                    errorMessages.Add(errorMessage);
                }
            });

            return errorMessages.Count == 0 ? null : string.Join(",", errorMessages);
        }

        private static string CheckProperty<T>(PropertyInfo property, T model)
        {
            if (!property.CanRead)
            {
                Console.WriteLine($"Can't check {property.Name}. It has no Get method");
                return null;
            }

            object value = property.GetValue(model);
            var checkAttribute = (BaseCheckAttribute)property.GetCustomAttribute(typeof(BaseCheckAttribute));

            if (checkAttribute != null)
            {
                var checker = checkAttribute.GetChecker();
                string[] valuesToCheck = Array.Empty<string>();
                switch (value)
                {
                    case IReadOnlyList<string> strings:
                        valuesToCheck = strings.ToArray();
                        break;
                    case string str:
                        valuesToCheck = new[] { str };
                        break;
                }

                foreach (string val in valuesToCheck)
                {
                    if (string.IsNullOrWhiteSpace(val))
                    {
                        var optionalAttribute = property.GetCustomAttribute(typeof(OptionalAttribute));
                        if (optionalAttribute == null)
                            throw new CheckFieldException(property.Name, val, "Empty setting value");
                        continue;
                    }

                    var checkResult = checker.CheckField(model, property.Name, val);
                    Console.WriteLine(checkResult.Description);
                    if (!checkResult.Result && checkResult.ThrowExceptionOnFail)
                        return checkResult.Description;
                }
            }
            else if (property.CanWrite)
            {
                object[] values = GetValuesToCheck(property, model);
                var errorMessages = new List<string>();
                foreach (object val in values)
                {
                    string errorMessage = ProcessChecks(val);
                    if (errorMessage != null)
                        errorMessages.Add(errorMessage);
                }
                return errorMessages.Count == 0 ? null : string.Join(",", errorMessages);
            }
            return null;
        }

        private static object[] GetValuesToCheck<T>(PropertyInfo property, T model)
        {
            var values = new List<object>();

            var getMethod = property.GetGetMethod();

            if (getMethod != null && getMethod.ReturnType != typeof(string)
                && typeof(IEnumerable).IsAssignableFrom(getMethod.ReturnType))
            {
                var arrayObject = getMethod.Invoke(model, null);

                if (arrayObject != null)
                {
                    foreach (object element in (IEnumerable)arrayObject)
                    {
                        values.Add(element);
                    }
                }
            }
            else if (getMethod != null && getMethod.ReturnType != typeof(string)
                && GetGenericArgumentsOfAssignableType(getMethod.ReturnType, typeof(IReadOnlyDictionary<,>)).Any())
            {
                var dictObject = (IDictionary)getMethod.Invoke(model, null);

                if (dictObject != null)
                {
                    foreach (object key in dictObject.Keys)
                    {
                        values.Add(dictObject[key]);
                    }
                }
            }
            else
            {
                if (property.PropertyType.IsClass && !property.PropertyType.IsValueType
                    && !property.PropertyType.IsPrimitive && property.PropertyType != typeof(string)
                    && property.PropertyType != typeof(object))
                {
                    values.Add(property.GetValue(model));
                }
            }

            return values.ToArray();
        }

        private static IReadOnlyList<Type> GetGenericArgumentsOfAssignableType(Type givenType, Type genericType)
        {
            while (true)
            {
                if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                {
                    return givenType.GenericTypeArguments;
                }

                var interfaceTypes = givenType.GetInterfaces();

                foreach (var it in interfaceTypes)
                {
                    if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    {
                        return it.GetGenericArguments();
                    }
                }

                var baseType = givenType.BaseType;
                if (baseType == null)
                {
                    return Array.Empty<Type>();
                }

                givenType = baseType;
            }
        }
    }
}
