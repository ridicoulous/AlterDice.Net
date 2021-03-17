using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDicePlaceOrderResponse:AlterDiceBaseResponse<AlterDiceOrderResponse>
    {
    }
    public class AlterDiceOrderResponse:ICommonOrderId
    {
        public AlterDiceOrderResponse()
        {

        }
        public AlterDiceOrderResponse(long id)
        {
            OrderId = id;
        }
        [JsonProperty("id")]
        public long OrderId { get; set; }
        [JsonIgnore]
        public string CommonId => OrderId.ToString();
    }
}
