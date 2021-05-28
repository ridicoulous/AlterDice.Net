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
        Buy = 0,
        Sell = 1
    }
    /// <summary>
    ///  Order status(0 - in process/1 - added to book/2 -done /3 - canceled).
    /// </summary>
    public enum AlterDiceOrderStatus
    {
        InProcess = 0,
        Active = 1,
        Filled = 2,
        Canceled = 3
    }
}
