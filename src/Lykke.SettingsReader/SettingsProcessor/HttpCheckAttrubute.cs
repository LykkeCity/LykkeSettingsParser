using System;

namespace Lykke.SettingsReader.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class HttpCheckAttribute :  Attribute
    {
        public HttpCheckAttribute(string path)
        {
            Url = path;
        }

        public string Url { get; }
    }
}
