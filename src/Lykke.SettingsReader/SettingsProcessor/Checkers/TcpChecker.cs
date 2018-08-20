using System.Reflection;
using Lykke.SettingsReader.Extensions;
using Lykke.SettingsReader.Helpers;

namespace Lykke.SettingsReader.Checkers
{
    internal class TcpChecker : ISettingsFieldChecker
    {
        private readonly string _portName;
        private readonly int _port;
        private readonly bool _isPortProvided;

        internal TcpChecker(string portName, int port)
        {
            _portName = portName;
            _port = port;
            _isPortProvided = !string.IsNullOrEmpty(_portName) || _port > 0;
        }

        public CheckFieldResult CheckField(object model, string propertyName, string value)
        {
            string address;
            int port;

            if (_isPortProvided)
            {
                address = value;

                if (string.IsNullOrEmpty(_portName))
                {
                    port = _port;
                }
                else
                {
                    var portProperty = model.GetType().GetTypeInfo().GetProperty(_portName);
                    if (portProperty == null)
                        return CheckFieldResult.Failed(propertyName, value, $"Property '{_portName}' not found");

                    var portValue = portProperty.GetValue(model).ToString();
                    
                    if (!int.TryParse(portValue, out port))
                        return CheckFieldResult.Failed(propertyName, value, $"Invalid port value in property '{_portName}'");
                }
            }
            else
            {
                if (value.SplitParts(':', 2, out var values))
                {
                    address = values[0];
                    
                    if (!int.TryParse(values[1], out port))
                        return CheckFieldResult.Failed(propertyName, value, "Invalid port");
                }
                else
                {
                    return CheckFieldResult.Failed(propertyName, value, "Invalid address");
                }
            }

            bool checkResult = TcpHelper.TcpCheck(address, port);

            string url = $"tcp://{address}:{port}";
            
            return checkResult
                ? CheckFieldResult.Ok(propertyName, url)
                : CheckFieldResult.Failed(propertyName, url);
        }
    }
}
