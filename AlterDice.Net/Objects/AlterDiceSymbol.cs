using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace AlterDice.Net.Objects
{
    public class AlterDiceSymbol: ICommonSymbol
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("pair")]
        public string Pair { get; set; }

        [JsonProperty("base")]
        public string BaseCurrency { get; set; }

        [JsonProperty("quote")]
        public string QuoteCurrency { get; set; }

        public string CommonName => Pair;

        public decimal CommonMinimumTradeSize => throw new System.NotImplementedException();
    }
}