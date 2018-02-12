using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AzureQueueCheckAttribute : BaseCheckAttribute
    {
        public AzureQueueCheckAttribute(bool throwExceptionOnFail = true)
            : base(throwExceptionOnFail)
        {
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new AzureQueueChecker(_throwExceptionOnFail);
        }
    }
}
