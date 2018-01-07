using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AmqpCheckAttribute : BaseCheckAttribute
    {
        public override ISettingsFieldChecker GetChecker()
        {
            return new AmqpChecker();
        }
    }
}
