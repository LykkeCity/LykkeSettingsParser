using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    public class SqlCheckAttribute : BaseCheckAttribute
    {
        public SqlCheckAttribute()
            : base(true)
        {
        }

        public SqlCheckAttribute(bool throwExceptionOnFail)
            : base(throwExceptionOnFail)
        {
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new SqlChecker(_throwExceptionOnFail);
        }
    }
}
