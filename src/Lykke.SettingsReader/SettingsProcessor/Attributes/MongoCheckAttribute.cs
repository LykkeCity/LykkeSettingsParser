using Lykke.SettingsReader.Checkers;

namespace Lykke.SettingsReader.Attributes
{
    /// <summary>
    /// Attribute for MongoDb connection string check
    /// </summary>
    public class MongoCheckAttribute : BaseCheckAttribute
    {
        /// <summary>
        /// Parameterless c-tor
        /// </summary>
        public MongoCheckAttribute()
            : base(true)
        {
        }
        
        /// <summary>
        /// C-tor with explicit throwExceptionOnFail flag
        /// </summary>
        public MongoCheckAttribute(bool throwExceptionOnFail)
            : base(throwExceptionOnFail)
        {
        }

        internal override ISettingsFieldChecker GetChecker()
        {
            return new MongoChecker(_throwExceptionOnFail);
        }
    }
}