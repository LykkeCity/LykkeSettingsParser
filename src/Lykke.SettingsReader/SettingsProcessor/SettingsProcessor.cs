using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lykke.SettingsReader.Attributes;
using Lykke.SettingsReader.Exceptions;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lykke.SettingsReader
{
    public static partial class SettingsProcessor
    {
        public static T Process<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new JsonStringEmptyException();
            }

            JToken jsonObj;
            try
            {
                jsonObj = (JToken)JsonConvert.DeserializeObject(json);
            }
            catch (Exception e)
            {
                throw new IncorrectJsonFormatException(e);
            }

            var result = FeelChildrenFields<T>(jsonObj);

            if (CheckPropertiesCount(result) > 0)
            {
                Console.WriteLine("Checking services");
                ProcessChecks(result);
            }

            return result;
        }

        private static T FeelChildrenFields<T>(JToken jsonObj, string path = "")
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

        private static object Convert_FromProperty(JProperty jsonObj, Type targetType, string path)
        {
            return Convert(jsonObj.Value, targetType, path);
        }

        public static bool IsGenericEnumerable(Type type)
        {
            return type.GetTypeInfo().IsGenericType &&
                type.GetTypeInfo().GetInterfaces().Any(
                ti => (ti == typeof(IEnumerable<>) || ti.Name == "IEnumerable"));
        }

        public static bool IsEnumerable(Type type)
        {
            return IsGenericEnumerable(type) || type.IsArray;
        }

        private static string ConcatPath(string path, string propertyName)
        {
            return $"{path}.{propertyName}".Trim('.');
        }

        private static int CheckPropertiesCount<T>(T model)
        {
            int count = 0;

            if (model == null)
                return count;

            Type objType = model.GetType();
            PropertyInfo[] properties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(model);
                var checkAttribute = (BaseCheckAttribute)property.GetCustomAttribute(typeof(BaseCheckAttribute));

                if (checkAttribute != null)
                {
                    count++;
                }
                else if (property.PropertyType.IsClass && !property.PropertyType.IsValueType &&
                         !property.PropertyType.IsPrimitive && property.PropertyType != typeof(string)
                         && property.PropertyType != typeof(object))
                {
                    count += CheckPropertiesCount(value);
                }
            }

            return count;
        }

        private static void ProcessChecks<T>(T model)
        {
            if (model == null)
                return;

            Type objType = model.GetType();
            PropertyInfo[] properties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(model);
                var checkAttribute = (BaseCheckAttribute)property.GetCustomAttribute(typeof(BaseCheckAttribute));

                if (checkAttribute != null)
                {
                    var checker = checkAttribute.GetChecker();

                    var checkResult = checker.CheckField(model, property, value);
                    Console.WriteLine(checkResult.Description);
                }
                else if (property.PropertyType.IsClass && !property.PropertyType.IsValueType &&
                    !property.PropertyType.IsPrimitive && property.PropertyType != typeof(string) 
                         && property.PropertyType != typeof(object))
                {
                    ProcessChecks(value);
                }
            }
        }
    }
}
