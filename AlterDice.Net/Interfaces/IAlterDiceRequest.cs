using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Interfaces
{
    public interface IAlterDiceAuthenticatedRequest
    {
        long RequestId { get; set; }
    }
}
