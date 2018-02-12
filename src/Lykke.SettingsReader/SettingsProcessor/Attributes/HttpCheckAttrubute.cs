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
        /// Default c-tor with relative uri parameter
        /// </summary>
        /// <param name="path">Relative uri</param>
        public HttpCheckAttribute(string path)
            : base(true)
        {
            Path = path;
        }

        /// <summary>
        /// C-tor with both relative uri and throwExceptionOnFail parameters
        /// </summary>
        /// <param name="path">Relative uri</param>
        /// <param name="throwExceptionOnFail">Throw exception on fail flag</param>
        public HttpCheckAttribute(string path, bool throwExceptionOnFail)
            : base(throwExceptionOnFail)
        {
            Path = path;
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new HttpChecker(Path, _throwExceptionOnFail);
        }
    }
}
