using AlterDice.Net.Converters;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDiceOrderTradesResponse : AlterDiceBaseResponse<AlterDiceActiveOrder> 
    {
    
    
    }

   
    public class AlterDiceOrderTrade : ICommonTrade
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("volume")]
        public decimal QuantityFilled { get; set; }

        [JsonProperty("price")]
        public decimal QuoteQuantityFilled { get; set; }

        [JsonProperty("time_create"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime TimeCreate { get; set; }

        [JsonProperty("rate")]
        public decimal Price { get; set; }

        [JsonProperty("commission")]
        public decimal Commission { get; set; }
        public string CommonId => Id.ToString();

        public decimal CommonPrice => Price;

        public decimal CommonQuantity => QuantityFilled;

        public decimal CommonFee => Commission;

        public string CommonFeeAsset => "";

        public DateTime CommonTradeTime => TimeCreate;
    }
}
