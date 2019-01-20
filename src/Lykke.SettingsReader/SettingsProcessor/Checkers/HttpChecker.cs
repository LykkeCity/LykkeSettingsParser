using System;
using System.Net.Http;
using Lykke.SettingsReader.Helpers;

namespace Lykke.SettingsReader.Checkers
{
    internal class HttpChecker : ISettingsFieldChecker
    {
        private readonly string _path;
        private readonly TimeSpan _timeout;

        internal HttpChecker(string path, TimeSpan timeout)
        {
            _path = path;
            _timeout = timeout;
        }

        public CheckFieldResult CheckField(object model, string propertyName, string value)
        {
            string url = GetFullUrl(value);
            
            if (string.IsNullOrEmpty(url))
                return CheckFieldResult.Failed(propertyName, url);

            try
            {
                bool checkResult;

                var httpClient = HttpClientProvider.Client;
                httpClient.Timeout = _timeout;

                using (var response = httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).GetAwaiter().GetResult())
                {
                    checkResult = response.IsSuccessStatusCode;
                }

                return checkResult
                    ? CheckFieldResult.Ok(propertyName, url)
                    : CheckFieldResult.Failed(propertyName, url);
            }
            catch (Exception)
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
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
