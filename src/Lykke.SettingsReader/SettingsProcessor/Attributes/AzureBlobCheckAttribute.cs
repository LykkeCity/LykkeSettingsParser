using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Attribute for azure blob connection string check
    /// </summary>
    public class AzureBlobCheckAttribute : BaseCheckAttribute
    {
        /// <summary>
        /// Parameterless c-tor
        /// </summary>
        public AzureBlobCheckAttribute()
            : base(true)
        {
        }

        /// <summary>
        /// Parameterless c-tor
        /// </summary>
        public AzureBlobCheckAttribute(bool throwExceptionOnFail)
            : base(throwExceptionOnFail)
        {
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new AzureBlobChecker(_throwExceptionOnFail);
        }
    }
}
