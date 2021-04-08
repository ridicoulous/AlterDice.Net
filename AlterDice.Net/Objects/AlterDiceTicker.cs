using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace AlterDice.Net.Objects
{
    public class AlterDiceTicker: ICommonTicker
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("pair")]
        public string Symbol { get; set; }

        [JsonProperty("last")]
        public decimal Last { get; set; }

        [JsonProperty("open")]
        public decimal Open { get; set; }

        [JsonProperty("close")]
        public decimal Close { get; set; }

        [JsonProperty("high")]
        public decimal High { get; set; }

        [JsonProperty("low")]
        public decimal Low { get; set; }

        [JsonProperty("volume_24H")]
        public decimal Volume { get; set; }

        public string CommonSymbol => Symbol;

        public decimal CommonHigh => High;

        public decimal CommonLow => Low;

        public decimal CommonVolume => Volume;
    }
}