using System;
using System.Net.Http;

namespace Lykke.SettingsReader.Checkers
{
    public sealed class HttpCheckerClient
    {
        static readonly HttpClient instance=new HttpClient{Timeout = TimeSpan.FromSeconds(5)};

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static HttpCheckerClient()
        {
        }

        HttpCheckerClient()
        {
        }

        public static HttpClient Instance => instance;
    }
}
