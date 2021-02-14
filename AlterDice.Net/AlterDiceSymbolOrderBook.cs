using CryptoExchange.Net.Objects;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.Sockets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces;

namespace AlterDice.Net
{
    public class AlterDiceSymbolOrderBook : SymbolOrderBook
    {
    
        private readonly AlterDiceSocketClient _socket;
        public AlterDiceSymbolOrderBook(string symbol, OrderBookOptions options) : base(symbol, options)
        {
           
        }

        public override void Dispose()
        {
            
        }

        protected override Task<CallResult<bool>> DoResync()
        {
            throw new NotImplementedException();
        }

        protected override  async Task<CallResult<UpdateSubscription>> DoStart()
        {
            _socket.OnOrderBookUpdate += _socket_OnOrderBookUpdate;
            WebsocketFactory wf = new WebsocketFactory();

            return new CallResult<UpdateSubscription>(new UpdateSubscription(new FakeConnection(_socket, wf.CreateWebsocket(log, "wss://echo.websocket.org")), null), null);

        }

        private void _socket_OnOrderBookUpdate(object obj)
        {
            
        }

        public class FakeConnection : SocketConnection
        {
            public FakeConnection(SocketClient client, IWebsocket socket) : base(client, socket)
            {
            }
        }
    }
}
