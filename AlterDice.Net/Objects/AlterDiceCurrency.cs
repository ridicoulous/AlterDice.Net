using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDiceBalancesResponse : AlterDiceBaseResponse<AlterDiceCurrenciesResult>
    {

    }
    public class AlterDiceCurrenciesResult
    {
        [JsonProperty("list")]
        public List<AlterDiceCurrencyResult> Result { get; set; }
    }
    public class AlterDiceCurrencyResult
    {
        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("balance_available")]
        public decimal BalanceAvailable { get; set; }

        [JsonProperty("currency")]
        public AlterDiceCurrency Currency { get; set; }
    }
    public class AlterDiceCurrency
    {
        [JsonProperty("iso3")]
        public string ShortName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
