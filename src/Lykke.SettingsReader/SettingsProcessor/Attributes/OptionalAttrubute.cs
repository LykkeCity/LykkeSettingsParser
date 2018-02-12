using System;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Attribute that allows empty value in settings object property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class OptionalAttribute :  Attribute
    {
    }
}
