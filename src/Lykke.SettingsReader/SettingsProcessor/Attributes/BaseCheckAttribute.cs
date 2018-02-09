using System;

namespace Lykke.SettingsReader.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class BaseCheckAttribute : Attribute
    {
        internal abstract ISettingsFieldChecker GetChecker();
    }
}
