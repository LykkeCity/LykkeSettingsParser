using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Lykke.SettingsReader.Checkers
{
    public sealed class HttpCheckerClient
    {
        static readonly HttpClient instance=new HttpClient();

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
