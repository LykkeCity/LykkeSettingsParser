using System.Reflection;
using Flurl;
using Flurl.Http;
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
            var result = new CheckFieldResult
            {
                Url = Url.Combine(value.ToString(), _path)
            };
                    
            if (!Url.IsValid(result.Url))
                throw new CheckFieldException(property.Name, value, "Invalid url");

            try
            {
                var resp = result.Url.GetAsync().Result;
                result.Result = resp.IsSuccessStatusCode;
            }
            catch
            {
                result.Result = false;
            }

            return result;
        }
    }
}
