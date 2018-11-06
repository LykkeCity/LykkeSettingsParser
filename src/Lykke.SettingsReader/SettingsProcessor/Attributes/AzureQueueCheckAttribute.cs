using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Attribute for azure queue connection string check
    /// </summary>
    public class AzureQueueCheckAttribute : BaseCheckAttribute
    {
        /// <inheritdoc />
        public override ISettingsFieldChecker GetChecker()
        {
            return new AzureQueueChecker();
        }
    }
}
