using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AzureTableCheckAttribute : BaseCheckAttribute
    {
        public override ISettingsFieldChecker GetChecker()
        {
            return new AzureTableChecker();
        }
    }
}
