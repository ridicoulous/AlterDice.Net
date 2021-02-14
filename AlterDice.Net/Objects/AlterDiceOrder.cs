using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDiceOrder
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type")]
        public AlterDiceOrderSide OrderSide{ get; set; }

        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("type_trade")]
        public AlterDiceOrderType OrderType { get; set; }

        [JsonProperty("pair")]
        public string Symbol { get; set; }

        [JsonProperty("volume")]
        public decimal Quantity { get; set; }

        [JsonProperty("volume_done")]
        public decimal QuantityDone { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("time_create"),JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("time_done"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime? ExecutedAt { get; set; }

        [JsonProperty("price_done")]
        public decimal? PriceDone { get; set; }
    }
}
