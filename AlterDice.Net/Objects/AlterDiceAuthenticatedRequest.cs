using AlterDice.Net.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDicePagedAuthenticatedRequest : AlterDiceAuthenticatedRequest
    {
        public AlterDicePagedAuthenticatedRequest(int page, int limit)
        {
            Page = page;
            Limit = limit;
        }
        public int Page { get; set; }
        public int Limit { get; set; }
    }
    public class AlterDiceAuthenticatedRequest : IAlterDiceAuthenticatedRequest
    {
        [JsonProperty("request_id")]
        public long RequestId { get; set; }
    }
}
