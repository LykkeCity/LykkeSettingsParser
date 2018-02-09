using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class SqlCheckAttribute : BaseCheckAttribute
    {
        public override ISettingsFieldChecker GetChecker()
        {
            return new SqlChecker();
        }
    }
}
