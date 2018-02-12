using System.Reflection;
using Lykke.SettingsReader.Exceptions;
using Lykke.SettingsReader.Extensions;
using Lykke.SettingsReader.Helpers;

namespace Lykke.SettingsReader.Checkers
{
    internal class TcpChecker : ISettingsFieldChecker
    {
        private readonly string _portName;
        private readonly int _port;
        private readonly bool _isPortProvided;
        private readonly bool _throwExceptionOnFail;

        internal TcpChecker(string portName, int port, bool throwExceptionOnFail)
        {
            _portName = portName;
            _port = port;
            _isPortProvided = !string.IsNullOrEmpty(_portName) || _port > 0;
            _throwExceptionOnFail = throwExceptionOnFail;
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
                        throw new CheckFieldException(propertyName, value, $"Property '{_portName}' not found");

                    var portValue = portProperty.GetValue(model).ToString();
                    if (!int.TryParse(portValue, out port))
                        throw new CheckFieldException(propertyName, value, $"Invalid port value in property '{_portName}'");
                }
            }
            else
            {
                if (value.SplitParts(':', 2, out var values))
                {
                    address = values[0];
                    if (!int.TryParse(values[1], out port))
                        throw new CheckFieldException(propertyName, value, "Invalid port");
                }
                else
                {
                    throw new CheckFieldException(propertyName, value, "Invalid address");
                }
            }

            bool checkResult = TcpHelper.TcpCheck(address, port);

            string url = $"tcp://{address}:{port}";
            return checkResult
                ? CheckFieldResult.Ok(propertyName, url)
                : CheckFieldResult.Failed(propertyName, url, _throwExceptionOnFail);
        }
    }
}
