using System;
using System.Net.Http;
using Lykke.SettingsReader.Exceptions;

namespace Lykke.SettingsReader.Checkers
{
    internal class HttpChecker : ISettingsFieldChecker
    {
        private readonly string _path;
        private readonly bool _throwExceptionOnFail;

        internal HttpChecker(string path, bool throwExceptionOnFail)
        {
            _path = path;
            _throwExceptionOnFail = throwExceptionOnFail;
        }

        public CheckFieldResult CheckField(object model, string propertyName, string value)
        {
            string url = GetFullUrl(value);
            if (string.IsNullOrEmpty(url))
                throw new CheckFieldException(propertyName, value, "Invalid url");

            try
            {
                using (var httpClient = new HttpClient{Timeout = TimeSpan.FromSeconds(5)})
                {
                    var response = httpClient.GetAsync(url).GetAwaiter().GetResult();
                    bool checkResult = response.IsSuccessStatusCode;
                    return checkResult
                        ? CheckFieldResult.Ok(propertyName, url)
                        : CheckFieldResult.Failed(propertyName, url, _throwExceptionOnFail);
                }

            }
            catch(Exception)
            {
                return CheckFieldResult.Failed(propertyName, url, _throwExceptionOnFail);
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
