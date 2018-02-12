using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AzureTableCheckAttribute : BaseCheckAttribute
    {
        public AzureTableCheckAttribute()
            : base(true)
        {
        }

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
