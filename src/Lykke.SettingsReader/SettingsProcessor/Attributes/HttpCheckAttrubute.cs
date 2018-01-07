using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class HttpCheckAttribute :  BaseCheckAttribute
    {
        public HttpCheckAttribute(string path)
        {
            Path = path;
        }

        public string Path { get; }
        
        public override ISettingsFieldChecker GetChecker()
        {
            return new HttpChecker(Path);
        }
    }
}
