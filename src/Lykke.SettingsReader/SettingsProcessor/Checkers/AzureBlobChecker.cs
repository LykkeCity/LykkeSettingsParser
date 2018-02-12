using Microsoft.WindowsAzure.Storage;

namespace Lykke.SettingsReader.Checkers
{
    internal class AzureBlobChecker : ISettingsFieldChecker
    {
        private readonly bool _throwExceptionOnFail;

        internal AzureBlobChecker(bool throwExceptionOnFail)
        {
            _throwExceptionOnFail = throwExceptionOnFail;
        }

        public CheckFieldResult CheckField(object model, string propertyName, string value)
        {
            string url = string.Empty;
            try
            {
                var account = CloudStorageAccount.Parse(value);
                var client = account.CreateCloudBlobClient();
                url = client.BaseUri.ToString();
                client.ListContainersSegmentedAsync(null).GetAwaiter().GetResult();
                return CheckFieldResult.Ok(propertyName, url);
            }
            catch
            {
                return CheckFieldResult.Failed(propertyName, url, _throwExceptionOnFail);
            }
        }
    }
}
