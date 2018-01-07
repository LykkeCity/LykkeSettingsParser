namespace Lykke.SettingsReader
{
    public class CheckFieldResult
    {
        public string Url { get; set; }
        public bool Result { get; set; }
        public string Description => $"Checking '{Url}' - {(Result ? "Ok" : "Failed")}";
    }
}
