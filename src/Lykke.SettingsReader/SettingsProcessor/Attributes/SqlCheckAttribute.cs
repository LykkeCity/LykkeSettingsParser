using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class SqlCheckAttribute : BaseCheckAttribute
    {
        public SqlCheckAttribute(bool throwExceptionOnFail = true)
            : base(throwExceptionOnFail)
        {
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new SqlChecker(_throwExceptionOnFail);
        }
    }
}
