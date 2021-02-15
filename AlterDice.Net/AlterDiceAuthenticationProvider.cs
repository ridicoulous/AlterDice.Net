using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace AlterDice.Net
{

    public class AlterDiceAuthenticationProvider : AuthenticationProvider
    {
        private static readonly object nonceLock = new object();
        private static long lastNonce;
        internal static string Nonce
        {
            get
            {
                lock (nonceLock)
                {
                    var nonce = (long)Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);
                    if (nonce == lastNonce)
                        nonce += 1;

                    lastNonce = nonce;
                    return lastNonce.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        public AlterDiceAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
        }

     

        public override Dictionary<string, string> AddAuthenticationToHeaders(string uri, HttpMethod method, Dictionary<string, object> parameters, bool signed, PostParameters postParameters, ArrayParametersSerialization arrayParametersSerialization)
        {       
            if (!signed)
                return new Dictionary<string, string>();

            var result = new Dictionary<string, string>();
            result.Add("login-token", Credentials.Key.GetString());
           // result.Add("X-Auth-Token", Credentials.Key.GetString());
            var dataToSign = CreateAuthPayload(parameters);
            var signedData = Sign(dataToSign);            
            result.Add("X-Auth-Sign", signedData);
            return result;
        }
        public override Dictionary<string, object> AddAuthenticationToParameters(string uri, HttpMethod method, Dictionary<string, object> parameters, bool signed, PostParameters postParameterPosition, ArrayParametersSerialization arraySerialization)
        {
            if (!signed)
                return parameters;

            if (!parameters.ContainsKey("request_id"))
            {
                parameters.Add("request_id", Nonce);
            }
            else
            {
                parameters["request_id"] = Nonce;
            }
            return parameters;
        }
        public string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
        public override string Sign(string toSign)
        {
            using (SHA256 sha = SHA256.Create())
            {
                return ByteArrayToString(sha.ComputeHash(Encoding.UTF8.GetBytes(toSign)));
            }
        }
        public string CreateAuthPayload(Dictionary<string, object> parameters)
        {
            Console.WriteLine($"{JsonConvert.SerializeObject(parameters)}");
            Console.WriteLine(String.Join("", parameters.OrderBy(p => p.Key).Select(p => p.Value)));
            return $"{String.Join("", parameters.OrderBy(p => p.Key).Select(p => p.Value))}{Credentials.Secret.GetString()}";
        }

    }
}
