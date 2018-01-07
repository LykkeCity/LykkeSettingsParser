using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using Flurl;
using Flurl.Http;
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

            ProcessChecks(result);

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

        private static void ProcessChecks<T>(T model)
        {
            var result = new StringBuilder();
            var properties = (from p in model.GetType().GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where p.CanWrite && p.CanRead && Attribute.IsDefined(p, typeof(BaseCheckAttribute)) select p).ToList();

            if (!properties.Any())
                return;

            Console.WriteLine("Checking services");

            foreach (var property in properties)
            {
                object value = property.GetValue(model);

                var checkAttribute = (BaseCheckAttribute)property.GetCustomAttribute(typeof(BaseCheckAttribute));

                var checker = checkAttribute.GetChecker();

                var checkResult = checker.CheckField(model, property, value);

                result.AppendLine(checkResult.Description);
            }

            Console.WriteLine(result.ToString());
        }
    }
}
