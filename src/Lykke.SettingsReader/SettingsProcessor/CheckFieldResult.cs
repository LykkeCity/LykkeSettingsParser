namespace Lykke.SettingsReader
{
    public class CheckFieldResult
    {
        public string Url { get; set; }
        public bool Result { get; set; }
        public string PropertyName { get; set; }
        public bool ThrowExceptionOnFail { get; set; }

        public string Description => $"Checking [{PropertyName}] on '{Url}' - {(Result ? "Ok" : "Failed")}";

        private CheckFieldResult()
        {
        }

        public static CheckFieldResult Ok(string propertyName, string url)
        {
            return new CheckFieldResult
            {
                Url = url,
                Result = true,
                PropertyName = propertyName,
            };
        }
        
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
    }
}
