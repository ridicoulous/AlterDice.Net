using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{

    public class AlterDiceGetOrdersResponse : AlterDiceBaseResponse<AlterDiceGetOrdersResult> { }
    public class AlterDiceGetActiveOrdersResponse : AlterDiceBaseResponse<AlterDiceGetActiveOrdersResult> { }

    public class AlterDiceGetActiveOrdersResult
    {
        [JsonProperty("list")]
        public List<AlterDiceActiveOrder> Orders { get; set; }
    }

    public  class AlterDiceGetOrdersResult
    {
        public AlterDiceGetOrdersResult()
        {

        }
        public AlterDiceGetOrdersResult(AlterDicePagination pagination, List<AlterDiceOrder> orders)
        {
            Pagination = pagination;
            Orders = orders;
        }
        [JsonProperty("pagination")]
        public AlterDicePagination Pagination { get; set; }
        [JsonProperty("list")]
        public List<AlterDiceOrder> Orders { get; set; }
    }

   
}
