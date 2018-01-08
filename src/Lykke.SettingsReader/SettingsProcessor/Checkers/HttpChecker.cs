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
            string val = value.ToString();

            var checkResult = new CheckFieldResult
            {
                Url = Url.Combine(val, _path)
            };

            if (!Url.IsValid(checkResult.Url))
                throw new CheckFieldException(property.Name, val, "Invalid url");

            try
            {
                var resp = checkResult.Url.GetAsync().Result;
                checkResult.Result = resp.IsSuccessStatusCode;
            }
            catch
            {
                checkResult.Result = false;
            }

            return checkResult;
        }
    }
}
