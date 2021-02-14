using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDiceOrderBookEntry : ISymbolOrderBookEntry
    {

        [JsonProperty("volume")]
        public decimal Quantity { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("rate")]
        public decimal Price { get; set; }

    }
}
