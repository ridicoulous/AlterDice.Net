using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlterDice.Net.Interfaces
{
    public interface IAlterDiceSocketClient
    {
        event Action<object> OnOrderBookUpdate;
        Task SubscribeToBook(string pair);
    }
}
