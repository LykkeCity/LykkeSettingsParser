using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AzureQueueCheckAttribute : BaseCheckAttribute
    {
        public override ISettingsFieldChecker GetChecker()
        {
            return new AzureQueueChecker();
        }
    }
}
