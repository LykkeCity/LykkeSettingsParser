using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lykke.SettingsReader.Test.Models.CheckAttributes;
using Xunit;

namespace Lykke.SettingsReader.Test
{
    public class HttpCheckTests
    {
        [Fact]
        public void Test()
        {
            var urls = new List<string>
            {
                "http://service.lykke-service.svc.cluster.local/",
            };

            var sb = new StringBuilder();
            sb.AppendLine("{");

            for (var i = 0; i < urls.Count; i++)
            {
                sb.AppendLine($"'Service{i+1}': '{urls[i]}',");
            }

            sb.AppendLine("}");
            
            var settings = SettingsProcessor.Process<TestHttpCheckServicesModel>(sb.ToString());

            string connString = "";
            string queueName = "";
            
            SettingsProcessor.CheckDependenciesAsync(settings, connString, queueName);
        }
    }
}
