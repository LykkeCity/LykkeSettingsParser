using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Attribute for RabbitMq connection string check
    /// </summary>
    public class AmqpCheckAttribute : BaseCheckAttribute
    {
        /// <inheritdoc />
        public override ISettingsFieldChecker GetChecker()
        {
            return new AmqpChecker();
        }
    }
}
