using System;
using Lykke.SettingsReader.Exceptions;
using Lykke.SettingsReader.Extensions;
using RabbitMQ.Client;

namespace Lykke.SettingsReader.Checkers
{
    internal class AmqpChecker : ISettingsFieldChecker
    {
        private readonly bool _throwExceptionOnFail;

        internal AmqpChecker(bool throwExceptionOnFail)
        {
            _throwExceptionOnFail = throwExceptionOnFail;
        }

        public CheckFieldResult CheckField(object model, string propertyName, string value)
        {
            ConnectionFactory factory;
            try
            {
                factory = new ConnectionFactory { Uri = value };
            }
            catch (Exception ex)
            {
                throw new CheckFieldException(propertyName, value, ex.Message);
            }
            var schema = factory.Ssl.Enabled ? "amqps" : "amqp";
            var url = $"{schema}://{factory.HostName}:{factory.Port}";

            try
            {
                using (var connection = factory.CreateConnection())
                {
                    bool checkResult = connection.IsOpen;
                    return checkResult
                        ? CheckFieldResult.Ok(propertyName, url)
                        : CheckFieldResult.Failed(propertyName, url, _throwExceptionOnFail);
                }
            }
            catch
            {
                return CheckFieldResult.Failed(propertyName, url, _throwExceptionOnFail);
            }
        }
    }
}
