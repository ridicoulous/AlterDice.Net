using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public partial class AlterDicePagination
    {
        [JsonProperty("page")]
        public int CurrentPage { get; set; }

        [JsonProperty("totalCountPage")]
        public int TotalPagesCount { get; set; }
    }

}
