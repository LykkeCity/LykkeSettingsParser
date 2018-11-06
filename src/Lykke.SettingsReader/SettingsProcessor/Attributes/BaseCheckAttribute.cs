using System;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Base class for attributes used for dependency check with property value
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class BaseCheckAttribute : Attribute
    {
        /// <summary>
        /// Gets a checker which can validate this attribute
        /// </summary>
        /// <returns></returns>
        public abstract ISettingsFieldChecker GetChecker();
    }
}
