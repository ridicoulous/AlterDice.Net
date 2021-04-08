using Newtonsoft.Json;

namespace AlterDice.Net.Objects
{
    public class AlterDiceActiveOrder: AlterDiceOrder
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

    }
}