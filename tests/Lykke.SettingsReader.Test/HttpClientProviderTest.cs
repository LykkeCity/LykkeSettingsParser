using System.Linq;
using System.Net.Http;
using Lykke.SettingsReader.Helpers;
using Xunit;


namespace Lykke.SettingsReader.Test
{
    public class HttpClientProviderTest
    {
        [Fact]
        public void NewHttpClientShouldBeCreatedAfterEachCall()
        {
            var clients = new HttpClient[42];
            
            for (var i = 0; i < clients.Length; i++)
            {
                clients[i] = HttpClientProvider.Client;
            }

            Assert.Equal
            (
                expected: clients.Length,
                actual: clients.Distinct().Count()
            );
        }
    }
}