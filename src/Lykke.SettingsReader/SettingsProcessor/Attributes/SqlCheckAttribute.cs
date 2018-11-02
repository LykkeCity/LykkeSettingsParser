using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Attribute for sql server connection string check
    /// </summary>
    public class SqlCheckAttribute : BaseCheckAttribute
    {
        /// <inheritdoc />
        public override ISettingsFieldChecker GetChecker()
        {
            return new SqlChecker();
        }
    }
}
