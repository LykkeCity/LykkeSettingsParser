using System.Reflection;
using Microsoft.WindowsAzure.Storage;

namespace Lykke.SettingsReader.Checkers
{
    internal class AzureTableChecker : ISettingsFieldChecker
    {
        public CheckFieldResult CheckField(object model, PropertyInfo property, object value)
        {
            string val = value.ToString();

            string url = string.Empty;
            try
            {
                var account = CloudStorageAccount.Parse(val);
                var client = account.CreateCloudTableClient();
                url = client.BaseUri.ToString();
                client.ListTablesSegmentedAsync(null).GetAwaiter().GetResult();
                return CheckFieldResult.Ok(url);
            }
            catch
            {
                return CheckFieldResult.Failed(url);
            }
        }
    }
}
