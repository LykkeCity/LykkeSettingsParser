using Lykke.SettingsReader.Exceptions;
using Lykke.SettingsReader.Extensions;
using RabbitMQ.Client;

namespace Lykke.SettingsReader.Checkers
{
    internal class AmqpChecker : ISettingsFieldChecker
    {
        public CheckFieldResult CheckField(object model, string propertyName, string value)
        {
            if (!value.SplitParts('@', 2, out var values) || !values[1].SplitParts(':', 2, out var amqpValues))
                throw new CheckFieldException(propertyName, value, "Invalid amqp connection string");

            if (!int.TryParse(amqpValues[1], out var port))
                throw new CheckFieldException(propertyName, value, "Invalid port");

            string address = amqpValues[0];
            string url = $"{address}:{port}";

            try
            {
                ConnectionFactory factory = new ConnectionFactory { Uri = value };
                using (var connection = factory.CreateConnection())
                {
                    bool checkResult = connection.IsOpen;
                    return checkResult ? CheckFieldResult.Ok(propertyName, url) : CheckFieldResult.Failed(propertyName, url);
                }
            }
            catch
            {
                return CheckFieldResult.Failed(propertyName, url);
            }
        }
    }
}
