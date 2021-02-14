using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDiceBaseResponse<TData>
    {
        [JsonProperty("status")]
        public bool Status { get; set; }
        [JsonProperty("data")]
        public TData Response { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("errors")]
        public List<string> Errors { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }

    }
}
