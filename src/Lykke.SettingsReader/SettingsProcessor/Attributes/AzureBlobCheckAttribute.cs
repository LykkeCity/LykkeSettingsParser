using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class AzureBlobCheckAttribute : BaseCheckAttribute
    {
        internal override ISettingsFieldChecker GetChecker()
        {
            return new AzureBlobChecker();
        }
    }
}
