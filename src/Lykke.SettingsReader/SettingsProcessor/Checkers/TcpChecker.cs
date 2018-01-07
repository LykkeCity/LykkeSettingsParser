﻿using System;
using System.Reflection;
using Lykke.SettingsReader.Exceptions;
using Lykke.SettingsReader.Extensions;
using Lykke.SettingsReader.Helpers;

namespace Lykke.SettingsReader.Checkers
{
    public class TcpChecker : ISettingsFieldChecker
    {
        private readonly string _portName;
        private readonly int _port;
        private readonly bool _isPortProvided;

        public TcpChecker(string portName, int port)
        {
            _portName = portName;
            _port = port;
            _isPortProvided = !string.IsNullOrEmpty(_portName) || _port > 0;
        }
        public CheckFieldResult CheckField(object model, PropertyInfo property, object value)
        {
            string address;
            int port;
            string val = value.ToString();

            if (_isPortProvided)
            {
                address = val;

                if (string.IsNullOrEmpty(_portName))
                {
                    port = _port;
                }
                else
                {
                    var portProperty = model.GetType().GetTypeInfo().GetProperty(_portName);

                    if (portProperty == null)
                        throw new CheckFieldException(property.Name, value, $"Property '{_portName}' not found");

                    var portValue = portProperty.GetValue(model).ToString();
                            
                    if (!int.TryParse(portValue, out port))
                        throw new CheckFieldException(property.Name, value, $"Invalid port value in property '{_portName}'");
                }
            }
            else
            {
                if (val.SplitParts(':', 2, out var values))
                {
                    address = values[0];
                    
                    if (!int.TryParse(values[1], out port))
                        throw new CheckFieldException(property.Name, value, "Invalid port");
                }
                else
                {
                    throw new CheckFieldException(property.Name, value, "Invalid address");
                }
            }

            return new CheckFieldResult
            {
                Url = $"{address}:{port}",
                Result = TcpHelper.TcpCheck(address, port)
            };
        }
    }
}