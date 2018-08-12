namespace Lykke.SettingsReader
{
    /// <summary>
    /// Result of field check
    /// </summary>
    public class CheckFieldResult
    {
        private string _error;
        
        /// <summary>Dependency url</summary>
        public string Url { get; set; }

        /// <summary>Check result</summary>
        public bool Result { get; set; }

        /// <summary>Field name</summary>
        public string PropertyName { get; set; }

        /// <summary>Check result for console output</summary>
        public string Description => string.IsNullOrEmpty(_error)
            ? $"Checking [{PropertyName}] on '{Url}' - {(Result ? "Ok" : "Failed")}"
            : _error;

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
                _error = null
            };
        }

        /// <summary>
        /// Helper method to init failed result
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="url">Dependency url</param>
        public static CheckFieldResult Failed(string propertyName, string url)
        {
            return new CheckFieldResult
            {
                Url = url,
                Result = false,
                PropertyName = propertyName,
                _error = null
            };
        }
        
        /// <summary>
        /// Helper method to init failed result
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// <param name="fieldValue">Field value</param>
        /// <param name="message">Error message</param>
        public static CheckFieldResult Failed(string propertyName, string fieldValue, string message)
        {
            return new CheckFieldResult
            {
                Url = null,
                Result = false,
                PropertyName = propertyName,
                _error = $"Check of the '{propertyName}' field value [{fieldValue}] is failed: {message}"
            };
        }

        private CheckFieldResult()
        {
        }
    }
}
