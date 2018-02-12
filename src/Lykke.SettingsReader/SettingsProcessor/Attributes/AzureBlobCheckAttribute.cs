using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AzureBlobCheckAttribute : BaseCheckAttribute
    {
        public AzureBlobCheckAttribute(bool throwExceptionOnFail = true)
            : base(throwExceptionOnFail)
        {
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new AzureBlobChecker(_throwExceptionOnFail);
        }
    }
}
