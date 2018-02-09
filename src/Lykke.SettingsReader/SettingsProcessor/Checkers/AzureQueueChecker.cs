using System.Reflection;
using Microsoft.WindowsAzure.Storage;
using Lykke.SettingsReader.Exceptions;

namespace Lykke.SettingsReader.Checkers
{
    internal class AzureQueueChecker : ISettingsFieldChecker
    {
        public CheckFieldResult CheckField(object model, PropertyInfo property, object value)
        {
            if (value == null)
                throw new CheckFieldException(property.Name, value, "Setting can not be null");

            string val = value.ToString();
            if (string.IsNullOrWhiteSpace(val))
                throw new CheckFieldException(property.Name, value, "Empty setting value");

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
