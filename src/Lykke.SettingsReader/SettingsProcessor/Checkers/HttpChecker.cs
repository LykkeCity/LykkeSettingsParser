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
            string url = GetFullUrl(val);
            bool checkResult;

            if (string.IsNullOrEmpty(url))
                throw new CheckFieldException(property.Name, val, "Invalid url");

            try
            {
                HttpResponseMessage response = HttpCheckerClient.Instance.GetAsync(url).GetAwaiter().GetResult();
                checkResult = response.IsSuccessStatusCode;
            }
            catch(Exception)
            {
                checkResult = false;
            }

            return checkResult
                ? CheckFieldResult.Ok(url)
                : CheckFieldResult.Failed(url);
        }

        private string GetFullUrl(string url)
        {
            try
            {
                return new Uri(new Uri(url), _path).ToString();
            }
            catch(Exception)
            {
                return string.Empty;
            }
        }
    }
}
