using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public enum AlterDiceOrderType
    {
        Limit = 0,
        Market = 1,
        Stop = 2,
        Limit_Quick_Market = 3
    }
    public enum AlterDiceOrderSide
    {
        Buy=0,
        Sell=1
    }
    public enum AlterDiceOrderStatus
    {
        Acvtive = 0,
        Filled = 2,
        Canceled=3
    }
}
