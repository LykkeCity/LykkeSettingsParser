namespace Lykke.SettingsReader
{
    /// <summary>
    /// Result of field check
    /// </summary>
    public class CheckFieldResult
    {
        /// <summary>Dependency url</summary>
        public string Url { get; set; }

        /// <summary>Check result</summary>
        public bool Result { get; set; }

        /// <summary>Field name</summary>
        public string PropertyName { get; set; }

        /// <summary>Exception throw flag</summary>
        public bool ThrowExceptionOnFail { get; set; }

        /// <summary>Check result for console output</summary>
        public string Description => $"Checking [{PropertyName}] on '{Url}' - {(Result ? "Ok" : "Failed")}";

        /// <summary>
        /// Helper method to init succes result
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="url">Dependency url</param>
        public static CheckFieldResult Ok(string propertyName, string url)
        {
            return new CheckFieldResult
            {
                Url = url,
                Result = true,
                PropertyName = propertyName,
            };
        }

        /// <summary>
        /// Helper method to init failed result
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="url">Dependency url</param>
        /// <param name="throwExceptionOnFail">Throw exception flag</param>
        public static CheckFieldResult Failed(string propertyName, string url, bool throwExceptionOnFail)
        {
            return new CheckFieldResult
            {
                Url = url,
                Result = false,
                PropertyName = propertyName,
                ThrowExceptionOnFail = throwExceptionOnFail,
            };
        }

        private CheckFieldResult()
        {
        }
    }
}
