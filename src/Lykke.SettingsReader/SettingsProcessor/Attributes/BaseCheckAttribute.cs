using System;

namespace Lykke.SettingsReader.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class BaseCheckAttribute : Attribute
    {
        protected readonly bool _throwExceptionOnFail;

        internal BaseCheckAttribute(bool throwExceptionOnFail)
        {
            _throwExceptionOnFail = throwExceptionOnFail;
        }

        internal abstract ISettingsFieldChecker GetChecker();
    }
}
