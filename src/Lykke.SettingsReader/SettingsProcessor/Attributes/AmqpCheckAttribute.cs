using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AmqpCheckAttribute : BaseCheckAttribute
    {
        public AmqpCheckAttribute()
            : base(true)
        {
        }

        public AmqpCheckAttribute(bool throwExceptionOnFail)
            : base(throwExceptionOnFail)
        {
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new AmqpChecker(_throwExceptionOnFail);
        }
    }
}
