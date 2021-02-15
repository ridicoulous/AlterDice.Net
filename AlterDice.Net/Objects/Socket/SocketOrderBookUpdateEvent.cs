using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects.Socket
{
    public class AlterDiceBaseSocketEvent<TData> 
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("room")]
        public string Room { get; set; }
        [JsonProperty("data")]
        public TData Data { get; set; }
    }

    public class AlterDiceSocketOrderBookUpdateEvent: AlterDiceBaseSocketEvent<AltertDiceSocketBookWrapper>
    {

    }
    

    public  class AltertDiceSocketBookWrapper
    {
        [JsonProperty("sell")]
        public Dictionary<long, AlterDiceOrderBookSide> Asks { get; set; }
        [JsonProperty("buy")]
        public Dictionary<long, AlterDiceOrderBookSide> Bids { get; set; }
    }

    public  class AlterDiceOrderBookSide
    {
        [JsonProperty("volume")]
        public long Volume { get; set; }

        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("rate")]
        public long Rate { get; set; }

        [JsonProperty("price")]
        public long Price { get; set; }
    }

}
