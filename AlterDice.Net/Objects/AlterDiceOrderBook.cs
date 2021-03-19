using CryptoExchange.Net.ExchangeInterfaces;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDiceOrderBook: ICommonOrderBook
    {
        [JsonProperty("buy")]
        public List<AlterDiceOrderBookEntry> Bids { get; set; }

        [JsonProperty("sell")]
        public List<AlterDiceOrderBookEntry> Asks { get; set; }
        [JsonIgnore]
        public IEnumerable<ISymbolOrderBookEntry> CommonBids => Bids;
        [JsonIgnore]
        public IEnumerable<ISymbolOrderBookEntry> CommonAsks => Asks;
    }
}
