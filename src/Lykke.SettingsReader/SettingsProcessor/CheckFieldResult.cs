namespace Lykke.SettingsReader
{
    public class CheckFieldResult
    {
        public string Url { get; set; }
        public bool Result { get; set; }
        public string Description => $"Checking '{Url}' - {(Result ? "Ok" : "Failed")}";

        private CheckFieldResult()
        {
            
        }

        public static CheckFieldResult Ok(string url)
        {
            return new CheckFieldResult
            {
                Url = url,
                Result = true
            };
        }
        
        public static CheckFieldResult Failed(string url)
        {
            return new CheckFieldResult
            {
                Url = url,
                Result = false
            };
        }
    }
}
