using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{

    public class AlterDiceGetOrdersResponse : AlterDiceBaseResponse<AlterDiceGetOrdersResult> { }
    public  class AlterDiceGetOrdersResult
    {
        [JsonProperty("list")]
        public List<AlterDiceOrder> Orders { get; set; }
    }

   
}
