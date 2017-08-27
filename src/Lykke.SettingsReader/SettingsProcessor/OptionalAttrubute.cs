using System;

namespace Lykke.SettingsReader
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class OptionalAttribute :  Attribute
    {
    }
}
