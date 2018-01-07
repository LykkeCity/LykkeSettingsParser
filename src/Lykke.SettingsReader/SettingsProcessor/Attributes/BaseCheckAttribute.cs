using System;

namespace Lykke.SettingsReader.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class BaseCheckAttribute : Attribute
    {
        public abstract ISettingsFieldChecker GetChecker();
    }
}
