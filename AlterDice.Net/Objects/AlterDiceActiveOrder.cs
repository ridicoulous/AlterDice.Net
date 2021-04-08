using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;

namespace AlterDice.Net.Objects
{
    public class AlterDiceActiveOrder : AlterDiceOrder,ICommonOrder
    {
        [JsonProperty("volume")]
        public new decimal Quantity { get; set; }
        [JsonProperty("volume_done")]
        public new decimal QuantityDone { get; set; }

        [JsonProperty("price")]
        public new decimal QuoteQuantity { get; set; }
        [JsonProperty("price_done")]
        public new decimal? QuoteQuantityFilled { get; set; }

        [JsonProperty("rate")]
        public new decimal Price { get; set; }

        public new string CommonId => Id.ToString();

        public new string CommonSymbol => Symbol;

        public new decimal CommonPrice => Price;

        public new decimal CommonQuantity => Quantity;

        public new string CommonStatus => Status.ToString();
    }
}