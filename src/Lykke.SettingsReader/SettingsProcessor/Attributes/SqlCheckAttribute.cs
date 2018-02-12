using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Attribute for sql server connection string check
    /// </summary>
    public class SqlCheckAttribute : BaseCheckAttribute
    {
        /// <summary>
        /// Parameterless c-tor
        /// </summary>
        public SqlCheckAttribute()
            : base(true)
        {
        }

        /// <summary>
        /// Parameterless c-tor
        /// </summary>
        public SqlCheckAttribute(bool throwExceptionOnFail)
            : base(throwExceptionOnFail)
        {
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new SqlChecker(_throwExceptionOnFail);
        }
    }
}
