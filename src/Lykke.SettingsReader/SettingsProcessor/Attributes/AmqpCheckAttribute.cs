using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AmqpCheckAttribute : BaseCheckAttribute
    {
        internal override ISettingsFieldChecker GetChecker()
        {
            return new AmqpChecker();
        }
    }
}
