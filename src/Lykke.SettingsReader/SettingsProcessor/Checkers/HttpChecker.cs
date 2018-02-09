using System;
using System.Net.Http;
using Lykke.SettingsReader.Exceptions;

namespace Lykke.SettingsReader.Checkers
{
    internal class HttpChecker : ISettingsFieldChecker
    {
        private readonly string _path;

        internal HttpChecker(string path)
        {
            _path = path;
        }

        public CheckFieldResult CheckField(object model, string propertyName, string value)
        {
            string url = GetFullUrl(value);
            if (string.IsNullOrEmpty(url))
                throw new CheckFieldException(propertyName, value, "Invalid url");

            try
            {
                HttpResponseMessage response = HttpCheckerClient.Instance.GetAsync(url).GetAwaiter().GetResult();
                bool checkResult = response.IsSuccessStatusCode;
                return checkResult ? CheckFieldResult.Ok(propertyName, url) : CheckFieldResult.Failed(propertyName, url);
            }
            catch(Exception)
            {
                return CheckFieldResult.Failed(propertyName, url);
            }
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
