using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AmqpCheckAttribute : BaseCheckAttribute
    {
        public AmqpCheckAttribute(bool throwExceptionOnFail = true)
            : base(throwExceptionOnFail)
        {
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new AmqpChecker(_throwExceptionOnFail);
        }
    }
}
