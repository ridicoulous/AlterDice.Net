using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net
{
    public class AlterDiceOrderBookOptions : OrderBookOptions
    {
        public readonly int? Timeout;
        public readonly int SymbolId;

        public AlterDiceOrderBookOptions(string name, int symbolId=0, int? timeOutToGet=null) : base(name, false, false)
        {
            SymbolId = symbolId;
            Timeout = timeOutToGet;
        }
    }
}
