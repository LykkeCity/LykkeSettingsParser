using System.Reflection;
using Microsoft.WindowsAzure.Storage;

namespace Lykke.SettingsReader.Checkers
{
    internal class AzureQueueChecker : ISettingsFieldChecker
    {
        public CheckFieldResult CheckField(object model, PropertyInfo property, object value)
        {
            string val = value.ToString();

            string url = string.Empty;
            try
            {
                var account = CloudStorageAccount.Parse(val);
                var client = account.CreateCloudQueueClient();
                url = client.BaseUri.ToString();
                client.ListQueuesSegmentedAsync(null).GetAwaiter().GetResult();
                return CheckFieldResult.Ok(url);
            }
            catch
            {
                return CheckFieldResult.Failed(url);
            }
        }
    }
}
