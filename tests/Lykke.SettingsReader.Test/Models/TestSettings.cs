namespace Lykke.SettingsReader.Test.Models
{
    internal class TestSettings<TValue>
    {
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public TValue Value { get; set; }

        public static TestSettings<TValue> FormatJsonString(string value)
        {
            if (null == value)
                return SettingsProcessor.ProcessAsync<TestSettings<TValue>>("{ 'value': null }").GetAwaiter().GetResult();

            value = value
                .Replace(@"\", @"\\")
                .Replace(@"""", @"\\""")
                .Replace(@"'", @"\\'");
            value = "\"" + value + "\"";
            return FormatJson(value);
        }

        public static TestSettings<TValue> FormatJson(string value)
        {
            return null == value
                ? SettingsProcessor.ProcessAsync<TestSettings<TValue>>("{ 'value': null }").GetAwaiter().GetResult()
                : SettingsProcessor.ProcessAsync<TestSettings<TValue>>("{ 'value': " + value + " }").GetAwaiter().GetResult();
        }
    }


    public class Settings
    {
        public PayApiSettings PayApi { get; set; }

    }
    public class PayApiSettings
    {
        public ServicesSettings Services { get; set; }
        public string HotWalletAddress { get; set; }
        public string LykkePayId { get; set; }
        public double SpredK { get; set; }
        public string LykkePayBaseUrl { get; set; }
        public int TransactionConfirmation { get; set; }
        public DbSettings Db { get; set; }
    }

    public class DbSettings
    {
        public string BitcoinAppRepository { get; set; }
    }

    public class ServicesSettings
    {
        public string MerchantAuthService { get; set; }

        public string PayServiceService { get; set; }

        public string GenerateAddressService { get; set; }

        public string StoreRequestService { get; set; }

        public string BitcoinApi { get; set; }

        public string MarketProfileService { get; set; }
        public string ExchangeOperationsService { get; internal set; }
        public string MerchantClientService { get; set; }
    }
}
