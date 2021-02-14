using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDiceLoginResponse : AlterDiceBaseResponse<AlterDiceLoginResult>
    {

    }
    public class AlterDiceLoginResult
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }
    }
}
