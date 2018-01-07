using System.Net.Sockets;

namespace Lykke.SettingsReader.Helpers
{
    internal static class TcpHelper
    {
        internal static bool TcpCheck(string address, int port)
        {
            bool result;

            try
            {
                using (var tcp = new TcpClient())
                {
                    tcp.Connect(address, port);
                    result = tcp.Connected;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
