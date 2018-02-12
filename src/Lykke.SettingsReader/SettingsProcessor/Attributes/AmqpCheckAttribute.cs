using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Attribute for RabbitMq connection string check
    /// </summary>
    public class AmqpCheckAttribute : BaseCheckAttribute
    {
        /// <summary>
        /// Parameterless c-tor
        /// </summary>
        public AmqpCheckAttribute()
            : base(true)
        {
        }

        /// <summary>
        /// C-tor with explicit throwExceptionOnFail flag
        /// </summary>
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
