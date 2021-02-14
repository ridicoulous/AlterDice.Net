using AlterDice.Net.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDiceAuthenticatedRequest : IAlterDiceAuthenticatedRequest
    {
        [JsonProperty("request_id")]
        public long RequestId { get; set; }
    }
}
