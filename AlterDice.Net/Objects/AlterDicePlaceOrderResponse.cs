using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDicePlaceOrderResponse:AlterDiceBaseResponse<AlterDiceOrderResponse>
    {
    }
    public class AlterDiceOrderResponse
    {
        [JsonProperty("id")]
        public long OrderId { get; set; }
    }
}
