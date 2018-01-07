﻿using System;
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
            var val = value.ToString();

            if (val.SplitParts('@', 2, out var values) && values[1].SplitParts(':', 2, out var amqpValues))
            {
                string address = amqpValues[0];

                if (!int.TryParse(amqpValues[1], out var port))
                    throw new CheckFieldException(property.Name, value, "Invalid port");

                var result = new CheckFieldResult {Url = $"{address}:{port}"};

                try
                {
                    ConnectionFactory factory = new ConnectionFactory {Uri = val};

                    using (var connection = factory.CreateConnection())
                    {
                        result.Result = connection.IsOpen;
                    }
                }
                catch
                {
                    result.Result = false;
                }
                

                return result;
            }

            throw new CheckFieldException(property.Name, value, "Invalid amqp connection string");
        }
    }
}