using System;
using System.Net.Http;
using System.Reflection;
using Lykke.SettingsReader.Exceptions;

namespace Lykke.SettingsReader.Checkers
{
    public class HttpChecker : ISettingsFieldChecker
    {
        private readonly string _path;

        public HttpChecker(string path)
        {
            _path = path;
        }

        public CheckFieldResult CheckField(object model, PropertyInfo property, object value)
        {
            string val = value.ToString();

            try
            {
                string url = new Uri(new Uri(val), _path).ToString();
                HttpResponseMessage response = HttpCheckerClient.Instance.GetAsync(url).GetAwaiter().GetResult();

                return new CheckFieldResult
                {
                    Url = url,
                    Result = response.IsSuccessStatusCode
                };
            }
            catch(UriFormatException)
            {
                throw new CheckFieldException(property.Name, val, "Invalid url");
            }
            catch(Exception)
            {
                return new CheckFieldResult{Result = false};
            }
        }
    }
}
