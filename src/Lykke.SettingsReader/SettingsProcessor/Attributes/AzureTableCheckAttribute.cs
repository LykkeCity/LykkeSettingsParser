using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Attribute for azure table connection string check
    /// </summary>
    public class AzureTableCheckAttribute : BaseCheckAttribute
    {
        /// <summary>
        /// Parameterless c-tor
        /// </summary>
        public AzureTableCheckAttribute()
            : base(true)
        {
        }

        /// <summary>
        /// Parameterless c-tor
        /// </summary>
        public AzureTableCheckAttribute(bool throwExceptionOnFail)
            : base(throwExceptionOnFail)
        {
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new AzureTableChecker(_throwExceptionOnFail);
        }
    }
}
