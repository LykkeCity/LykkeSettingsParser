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
            var attributeTypes = new List<Type>
            {
                typeof(HttpCheckAttribute),
                typeof(TcpCheckAttribute),
                typeof(AmqpCheckAttribute)
            };

            var properties = (from p in model.GetType().GetTypeInfo().GetProperties()
                where p.CanWrite && p.CanRead && attributeTypes.Any(item => Attribute.IsDefined(p, item)) select p).ToList();

            if (!properties.Any())
                return;

            Console.WriteLine("Checking services");

            foreach (var property in properties)
            {
                string value = property.GetValue(model).ToString();
                string url = string.Empty;
                string address;
                int port;
                bool checkResult = false;

                var checkAttribute = property.GetCustomAttribute(typeof(Attribute));

                switch (checkAttribute)
                {
                    case HttpCheckAttribute httpCheck:
                        url = Url.Combine(value, httpCheck.Url);
                    
                        if (!Url.IsValid(url))
                            throw new CheckFieldException(property.Name, "Wrong url");

                        checkResult = HttpCheck(url);
                        break;
                    case TcpCheckAttribute tcpCheck:
                    {
                        if (tcpCheck.IsPortProvided)
                        {
                            address = value;

                            if (string.IsNullOrEmpty(tcpCheck.PortName))
                            {
                                port = tcpCheck.Port;
                            }
                            else
                            {
                                var portProperty = model.GetType().GetTypeInfo().GetProperty(tcpCheck.PortName);

                                if (portProperty == null)
                                    throw new CheckFieldException(property.Name, $"Property '{tcpCheck.PortName}' not found");

                                var portValue = portProperty.GetValue(model).ToString();
                            
                                if (!int.TryParse(portValue, out port))
                                    throw new CheckFieldException(property.Name, $"Wrong port value in property '{tcpCheck.PortName}'");
                            }
                        }
                        else
                        {
                            if (SplitParts(value, ':', 2, out var values))
                            {
                                address = values[0];
                                port = System.Convert.ToInt32(values[1]);
                            }
                            else
                            {
                                throw new CheckFieldException(property.Name, "Wrong address");
                            }
                        }

                        url = $"{address}:{port}";
                        checkResult = TcpCheck(address, port);
                        break;
                    }
                    case AmqpCheckAttribute amqpCheck:
                    {
                        if (SplitParts(value, '@', 2, out var values) && SplitParts(values[1], ':', 2, out var amqpValues))
                        {
                            address = amqpValues[0];
                            port = System.Convert.ToInt32(amqpValues[1]);
                        }
                        else
                        {
                            throw new CheckFieldException(property.Name, "Wrong amqp connection string");
                        }

                        url = $"{address}:{port}";
                        checkResult = TcpCheck(address, port);
                        break;
                    }
                }

                result.AppendLine($"Checking '{url}' - {(checkResult ? "OK" : "Failed")}");
            }

            Console.WriteLine(result.ToString());
        }

        private static bool SplitParts(string value, char splitChar, int length, out string[] values)
        {
            values = value.Split(splitChar);
            return values != null && values.Length == length;
        }

        private static bool HttpCheck(string url)
        {
            try
            {
                var resp = url.GetAsync().Result;
                return resp.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        
        private static bool TcpCheck(string address, int port)
        {
            bool result;

            try
            {
                using (var tcp = new TcpClient())
                {
                    tcp.Connect(address, port);
                    result = tcp.Connected;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
