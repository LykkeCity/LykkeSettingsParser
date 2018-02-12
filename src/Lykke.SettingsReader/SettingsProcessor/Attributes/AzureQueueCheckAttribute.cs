using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AzureQueueCheckAttribute : BaseCheckAttribute
    {
        public AzureQueueCheckAttribute()
            : base(true)
        {
        }

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
