using System.Reflection;
using Lykke.SettingsReader.Exceptions;
using Lykke.SettingsReader.Extensions;
using RabbitMQ.Client;

namespace Lykke.SettingsReader.Checkers
{
    public class AmqpChecker : ISettingsFieldChecker
    {
        public CheckFieldResult CheckField(object model, PropertyInfo property, object value)
        {
            string val = value.ToString();

            if (val.SplitParts('@', 2, out var values) && values[1].SplitParts(':', 2, out var amqpValues))
            {
                string address = amqpValues[0];

                if (!int.TryParse(amqpValues[1], out var port))
                    throw new CheckFieldException(property.Name, val, "Invalid port");

                string url = $"{address}:{port}";
                bool checkResult;

                try
                {
                    ConnectionFactory factory = new ConnectionFactory {Uri = val};

                    using (var connection = factory.CreateConnection())
                    {
                        checkResult = connection.IsOpen;
                    }
                }
                catch
                {
                    checkResult = false;
                }
                
                return checkResult
                    ? CheckFieldResult.Ok(url)
                    : CheckFieldResult.Failed(url);
            }

            throw new CheckFieldException(property.Name, val, "Invalid amqp connection string");
        }
    }
}
