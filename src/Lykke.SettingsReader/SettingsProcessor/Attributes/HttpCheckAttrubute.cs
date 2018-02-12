using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class HttpCheckAttribute :  BaseCheckAttribute
    {
        public string Path { get; }

        public HttpCheckAttribute(string path, bool throwExceptionOnFail = true)
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
