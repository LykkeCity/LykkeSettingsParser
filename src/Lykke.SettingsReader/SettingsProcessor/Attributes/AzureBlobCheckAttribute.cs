using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AzureBlobCheckAttribute : BaseCheckAttribute
    {
        public override ISettingsFieldChecker GetChecker()
        {
            return new AzureBlobChecker();
        }
    }
}
