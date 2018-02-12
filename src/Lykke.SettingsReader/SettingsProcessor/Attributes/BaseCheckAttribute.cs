using System;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Base class for attributes used for dependency check with property value
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class BaseCheckAttribute : Attribute
    {
        /// <summary>Flag that defines whether exception should be thrown on check failure</summary>
        protected readonly bool _throwExceptionOnFail;

        internal BaseCheckAttribute(bool throwExceptionOnFail)
        {
            _throwExceptionOnFail = throwExceptionOnFail;
        }

        internal abstract ISettingsFieldChecker GetChecker();
    }
}
