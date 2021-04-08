using Newtonsoft.Json;

namespace AlterDice.Net.Objects
{
    public class AlterDiceActiveOrder: AlterDiceOrder
    {
        [JsonProperty("volume")]
        public override decimal Quantity { get; set; }
        [JsonProperty("volume_done")]
        public override decimal QuantityDone { get; set; }

        [JsonProperty("price")]
        public override decimal QuoteQuantity { get; set; }
        [JsonProperty("price_done")]
        public override decimal? QuoteQuantityFilled { get; set; }

        [JsonProperty("rate")]
        public override decimal Price { get; set; }

    }
}