using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AlterDice.Net
{
    public class AlterDiceClientOptions : RestClientOptions
    {
        public readonly string Login, Password;
        public AlterDiceClientOptions() : base("https://api.alterdice.com/v1/")
        {
            this.LogVerbosity = CryptoExchange.Net.Logging.LogVerbosity.Debug;
            LogWriters = new List<System.IO.TextWriter>() { new DebugTextWriter() };

        }
        public AlterDiceClientOptions(string login, string password) : base("https://api.alterdice.com/v1/")
        {
            Login = login;
            Password = password;
            this.LogVerbosity = CryptoExchange.Net.Logging.LogVerbosity.Debug;
            LogWriters = new List<System.IO.TextWriter>() { new DebugTextWriter() };

        }
        public AlterDiceClientOptions(HttpClient client) : base(client, "https://api.alterdice.com/v1/")
        {
            this.LogVerbosity = CryptoExchange.Net.Logging.LogVerbosity.Debug;
            LogWriters = new List<System.IO.TextWriter>() { new DebugTextWriter() };
        }

        public void SetApiCredentials(ApiCredentials credentials)
        {
            ApiCredentials = credentials;
        }
    }
}
