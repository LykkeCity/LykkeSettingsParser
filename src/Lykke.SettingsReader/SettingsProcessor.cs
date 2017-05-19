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
    public static class SettingsProcessor
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

            return result;
        }

        private static T FeelChildrenFields<T>(JToken jsonObj, string path = "")
        {
            T result;
            if (jsonObj.Type == JTokenType.Object)
            {
                result = (T)Activator.CreateInstance(typeof(T));
                var properties = (from p in typeof(T).GetProperties()
                                  where p.CanWrite && p.CanRead
                                  select p).ToList();
                foreach (var pr in properties)
                {
                    var jProp = ((JObject)jsonObj).Properties().FirstOrDefault(jp => jp.Name.Equals(pr.Name, StringComparison.CurrentCultureIgnoreCase));
                    if (jProp == null)
                    {
                        if (pr.GetCustomAttributes(typeof(OptionalAttribute), false).Any())
                        {
                            continue;
                        }

                        throw new RequaredFieldEmptyException
                        {
                            FieldName = $"{path}.{pr.Name}".Trim('.')
                        };
                    }

                    var method = typeof(SettingsProcessor).GetMethod("FeelChildrenFields", BindingFlags.Static | BindingFlags.NonPublic);
                    var genericMethod = method.MakeGenericMethod(pr.PropertyType);
                    try
                    {
                        var jResult = genericMethod.Invoke(null, new[] { (object)jProp, $"{path}.{pr.Name}".Trim('.') });
                        pr.SetValue(result, jResult);
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }

                }
            }
            else if (jsonObj.Type == JTokenType.Array)
            {
                if (!IsEnumerable(typeof(T)))
                {
                    throw new RequaredFieldEmptyException
                    {
                        FieldName = $"{path}.{((JArray)jsonObj)}".Trim('.')
                    };
                }
                var childType = typeof(T).IsArray
                        ? typeof(T).GetElementType()
                        : typeof(T).GenericTypeArguments.First();

                var concreteType = typeof(List<>).MakeGenericType(childType);
                var res = (IList)Activator.CreateInstance(concreteType);

                int i = 0;
                foreach (var elem in (JArray)jsonObj)
                {

                    var method = typeof(SettingsProcessor).GetMethod("FeelChildrenFields", BindingFlags.Static | BindingFlags.NonPublic);
                    var genericMethod = method.MakeGenericMethod(childType);
                    try
                    {
                        var jResult = genericMethod.Invoke(null, new[] { (object)elem, $"{path}.{i}".Trim('.') });
                        res.Add(jResult);
                    }
                    catch (TargetInvocationException e)
                    {
                        throw e.InnerException;
                    }

                    i++;
                }


                if (typeof(T).IsArray)
                {
                    var arr = Array.CreateInstance(typeof(T).GetElementType(), res.Count);
                    for (var ii = 0; ii < res.Count; ii++)
                    {
                        arr.SetValue(res[ii], ii);
                    }
                    result = (T)(object)arr;

                }
                else
                {
                    result = (T)res;
                }
            }
            else if (jsonObj.Type == JTokenType.Property)
            {
                var method = typeof(SettingsProcessor).GetMethod("FeelChildrenFields", BindingFlags.Static | BindingFlags.NonPublic);
                var genericMethod = method.MakeGenericMethod(typeof(T));
                try
                {
                    result = (T)genericMethod.Invoke(null, new[] { (object)((JProperty)jsonObj).Value, $"{path}".Trim('.') });
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }

            }
            else
            {
                result = (T)Convert.ChangeType(((JValue)jsonObj).Value, typeof(T));
            }

            return result;
        }

        public static bool IsGenericEnumerable(Type type)
        {
            return type.GetTypeInfo().IsGenericType &&
                type.GetInterfaces().Any(
                ti => (ti == typeof(IEnumerable<>) || ti.Name == "IEnumerable"));
        }

        public static bool IsEnumerable(Type type)
        {
            return IsGenericEnumerable(type) || type.IsArray;
        }
    }
}
