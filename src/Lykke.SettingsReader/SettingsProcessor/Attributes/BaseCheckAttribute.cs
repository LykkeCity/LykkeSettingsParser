using System;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Base class for attributes used for dependency check with property value
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class BaseCheckAttribute : Attribute
    {
        internal abstract ISettingsFieldChecker GetChecker();
    }
}
