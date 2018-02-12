using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Attribute for azure queue connection string check
    /// </summary>
    public class AzureQueueCheckAttribute : BaseCheckAttribute
    {
        /// <summary>
        /// Parameterless c-tor
        /// </summary>
        public AzureQueueCheckAttribute()
            : base(true)
        {
        }

        /// <summary>
        /// Parameterless c-tor
        /// </summary>
        public AzureQueueCheckAttribute(bool throwExceptionOnFail)
            : base(throwExceptionOnFail)
        {
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new AzureQueueChecker(_throwExceptionOnFail);
        }
    }
}
