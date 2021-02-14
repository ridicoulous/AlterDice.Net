using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDiceGetOrderRequest:AlterDiceAuthenticatedRequest
    {
        public AlterDiceGetOrderRequest(long id)
        {
            OrderId = id;
        }
        [JsonProperty("order_id")]
        public long OrderId { get; set; }
    }
}
