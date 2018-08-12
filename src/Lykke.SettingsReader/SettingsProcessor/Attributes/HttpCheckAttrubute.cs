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
        {
            Path = path;
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new HttpChecker(Path);
        }
    }
}
