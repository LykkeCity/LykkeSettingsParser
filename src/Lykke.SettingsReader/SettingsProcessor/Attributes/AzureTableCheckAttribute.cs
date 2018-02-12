using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AzureTableCheckAttribute : BaseCheckAttribute
    {
        public AzureTableCheckAttribute(bool throwExceptionOnFail = true)
            : base(throwExceptionOnFail)
        {
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new AzureTableChecker(_throwExceptionOnFail);
        }
    }
}
