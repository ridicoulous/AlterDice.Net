using AlterDice.Net.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDicePlaceOrderRequest : AlterDiceAuthenticatedRequest
    {

        [JsonProperty("type_trade")]
        public int OrderTypeSerialized => (int)OrderType;
        [JsonIgnore]
        public AlterDiceOrderType OrderType { get; set; }
        [JsonProperty("type")]
        public int OrderSideSerialized => (int)OrderSide;
        [JsonIgnore]
        public AlterDiceOrderSide OrderSide { get; set; }
        /// <summary>
        /// Rate (require for Limit and Stop Limit type_trade)        
        /// </summary>
        [JsonProperty("rate")]
        public decimal? Price { get; set; }
        [JsonProperty("volume")]
        public decimal Quantity { get; set; }
        public string Symbol { get; set; }

    }

    /*
     type_trade	int	
Limit/Market/Stop Limit/Quick Market (0/1/2/3)

type	int	
Buy/Sell (0/1)

rate	float	
Rate (require for Limit and Stop Limit type_trade)

stop_rate	float	
Rate (require for Stop Limit type_trade)

volume	float	
Volume

pair	int	
Currency pair (BTCUSD).

request_id	string	
Request id
     
     */
}
