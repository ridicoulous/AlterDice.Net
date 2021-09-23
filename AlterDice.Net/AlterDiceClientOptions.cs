using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AlterDice.Net
{
    public class AlterDiceClientOptions : RestClientOptions
    {
        public readonly string Login, Password;
        public AlterDiceClientOptions() : base("https://api.alterdice.com/")
        {
            this.LogLevel = Microsoft.Extensions.Logging.LogLevel.Debug;
            LogWriters = new List<ILogger> { new DebugLogger() };

        }
        public AlterDiceClientOptions(string login, string password) : base("https://api.alterdice.com/")
        {
            Login = login;
            Password = password;
            this.LogLevel = Microsoft.Extensions.Logging.LogLevel.Debug;
            LogWriters = new List<ILogger> { new DebugLogger() };
        }
        public AlterDiceClientOptions(HttpClient client) : base(client, "https://api.alterdice.com/")
        {
            this.LogLevel = Microsoft.Extensions.Logging.LogLevel.Debug;
            LogWriters = new List<ILogger> { new DebugLogger() };
        }

        public void SetApiCredentials(ApiCredentials credentials)
        {
            ApiCredentials = credentials;
        }
    }
}
