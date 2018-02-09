using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AzureQueueCheckAttribute : BaseCheckAttribute
    {
        internal override ISettingsFieldChecker GetChecker()
        {
            return new AzureQueueChecker();
        }
    }
}
