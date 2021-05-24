using AlterDice.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDiceOrderTradesResponse : AlterDiceBaseResponse<AlterDiceOrderTradeResponseWrapper> 
    {
    
    
    }

    public class AlterDiceOrderTradeResponseWrapper 
    {
        [JsonProperty("order")]
        public AlterDiceOrder Order { get; set; }

        [JsonProperty("list")]
        public List<AlterDiceOrderTrade> Trades { get; set; }

    }

    public class AlterDiceOrderTrade : ICommonTrade
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("volume"),JsonConverter(typeof(Decimal10e8Converter))]
        public decimal QuantityFilled { get; set; }

        [JsonProperty("price"), JsonConverter(typeof(Decimal10e8Converter))]
        public decimal QuoteQuantityFilled { get; set; }

        [JsonProperty("time_create")]
        public DateTime TimeCreate { get; set; }

        [JsonProperty("rate"), JsonConverter(typeof(Decimal10e8Converter))]
        public decimal Price { get; set; }

        [JsonProperty("commission"), JsonConverter(typeof(Decimal10e8Converter))]
        public decimal Commission { get; set; }
        public string CommonId => Id.ToString();

        public decimal CommonPrice => Price;

        public decimal CommonQuantity => QuantityFilled;

        public decimal CommonFee => Commission;

        public string CommonFeeAsset => "";
    }
}
