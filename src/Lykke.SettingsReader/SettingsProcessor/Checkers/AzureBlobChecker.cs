using System.Reflection;
using Microsoft.WindowsAzure.Storage;

namespace Lykke.SettingsReader.Checkers
{
    internal class AzureBlobChecker : ISettingsFieldChecker
    {
        public CheckFieldResult CheckField(object model, PropertyInfo property, object value)
        {
            string val = value.ToString();

            string url = string.Empty;
            try
            {
                var account = CloudStorageAccount.Parse(val);
                var client = account.CreateCloudBlobClient();
                url = client.BaseUri.ToString();
                client.ListContainersSegmentedAsync(null).GetAwaiter().GetResult();
                return CheckFieldResult.Ok(url);
            }
            catch
            {
                return CheckFieldResult.Failed(url);
            }
        }
    }
}
