using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net
{
    public class AlterDiceOrderBookOptions : OrderBookOptions
    {
        public readonly int Timeout;
        public AlterDiceOrderBookOptions(string name, int timeOutToGet) : base(name, false, false)
        {
            Timeout = timeOutToGet;
        }
    }
}
