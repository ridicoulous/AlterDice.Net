using AlterDice.Net.Objects.Socket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlterDice.Net.Interfaces
{
    public interface IAlterDiceSocketClient
    {
        event Action<AlterDiceSocketOrderBookUpdateEvent> OnOrderBookUpdate;
        Task SubscribeToBook(int symbolId);
    }
}
