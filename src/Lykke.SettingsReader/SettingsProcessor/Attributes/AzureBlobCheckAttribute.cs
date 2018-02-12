using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AzureBlobCheckAttribute : BaseCheckAttribute
    {
        public AzureBlobCheckAttribute()
            : base(true)
        {
        }

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
