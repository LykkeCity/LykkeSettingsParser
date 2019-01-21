using System;
using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Attribute for url check with relative uri specified in Path parameter using http get
    /// </summary>
    public class HttpCheckAttribute :  BaseCheckAttribute
    {
        /// <summary>
        /// Relative uri
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Timeout for http request
        /// </summary>
        public TimeSpan Timeout { get; }

        /// <summary>
        /// Default c-tor with relative uri parameter
        /// </summary>
        /// <param name="path">Relative uri</param>
        /// <param name="timeoutSeconds">Timeout in seconds for httpRequest</param>
        public HttpCheckAttribute(string path, int timeoutSeconds = 10)
        {
            Path = path;
            Timeout = TimeSpan.FromSeconds(timeoutSeconds);
        }

        /// <inheritdoc />
        public override ISettingsFieldChecker GetChecker()
        {
            return new HttpChecker(Path, Timeout);
        }
    }
}
