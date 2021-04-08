using System;
using System.Collections.Generic;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace AlterDice.Net.Objects
{
    public class AlterDicePublicTradeResponse : AlterDiceBaseResponse<List<AlterDicePublicTrade>>
    {
        
    }

    public class AlterDicePublicTrade : ICommonRecentTrade
    {
        [JsonProperty("volume")]
        public decimal Volume { get; set; }

        [JsonProperty("rate")]
        public decimal Price { get; set; }

        [JsonProperty("price")]
        public decimal QuoteQuantity { get; set; }

        [JsonProperty("timestamp"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime Timestamp { get; set; }

        [JsonProperty("type")]
        public AlterDiceOrderSide Type { get; set; }

        public decimal CommonPrice => Price;

        public decimal CommonQuantity => Volume;

        public DateTime CommonTradeTime => Timestamp;
    }
}