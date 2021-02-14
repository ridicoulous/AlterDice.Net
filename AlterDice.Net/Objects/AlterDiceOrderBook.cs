using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDiceOrderBook
    {
        [JsonProperty("buy")]
        public List<AlterDiceOrderBookEntry> Bids { get; set; }

        [JsonProperty("sell")]
        public List<AlterDiceOrderBookEntry> Asks { get; set; }
    }
}
