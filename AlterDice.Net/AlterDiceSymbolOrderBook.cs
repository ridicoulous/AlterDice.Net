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
using System.Timers;
using System.Threading;

namespace AlterDice.Net
{
    public class AlterDiceSymbolOrderBook : SymbolOrderBook
    {
        private readonly System.Timers.Timer timer;
        private readonly AlterDiceSocketClient _socket;
        private readonly AlterDiceClient _apiClient;
        public AlterDiceSymbolOrderBook(string symbol, AlterDiceClient client, AlterDiceOrderBookOptions opts) : this(symbol, opts)
        {
            _apiClient = client;
        }
        public AlterDiceSymbolOrderBook(string symbol, AlterDiceOrderBookOptions options) : base(symbol, options)
        {
            _apiClient = new AlterDiceClient();
            timer = new System.Timers.Timer(options.Timeout);
            timer.Elapsed += T_Elapsed;
            _socket = new AlterDiceSocketClient("asd",new SocketClientOptions("https://socket.alterdice.com"),new AlterDiceAuthenticationProvider(new CryptoExchange.Net.Authentication.ApiCredentials("42","42")));
            _socket.OnOrderBookUpdate += _socket_OnOrderBookUpdate1;
        }

        private void _socket_OnOrderBookUpdate1(object obj)
        {
            
        }

        private CancellationTokenSource cts = new CancellationTokenSource();

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            //cts.Cancel();
            //cts = new CancellationTokenSource();

            Task.Run(async () => await GetAndSetBook());
        }

        private async Task<CallResult<bool>> GetAndSetBook()
        {
            try
            {
                //ct.ThrowIfCancellationRequested();
                var book = await _apiClient.GetOrderBookAsync(Symbol);
                if (book)
                {
                    SetInitialOrderBook(DateTime.UtcNow.Ticks, book.Data.Bids, book.Data.Asks);
                    return new CallResult<bool>(true, null);
                }
                return new CallResult<bool>(false, book.Error);

            }
            catch (Exception ex)
            {
                log.Write(CryptoExchange.Net.Logging.LogVerbosity.Error, ex.ToString());
                return new CallResult<bool>(false, new ServerError(ex.ToString()));
            }
        }

        public override void Dispose()
        {

        }

        protected override async Task<CallResult<bool>> DoResync()
        {
               
            return  await GetAndSetBook();
        }

        protected override async Task<CallResult<UpdateSubscription>> DoStart()
        {
            //_socket.OnOrderBookUpdate += _socket_OnOrderBookUpdate;
            WebsocketFactory wf = new WebsocketFactory();
            //  timer.Start();
            
            await _socket.SubscribeToBook("sd");

            return new CallResult<UpdateSubscription>(new UpdateSubscription(new FakeConnection(_socket, wf.CreateWebsocket(log, "wss://echo.websocket.org")), null), null);
        }

   

        public class FakeConnection : SocketConnection
        {
            public FakeConnection(SocketClient client, IWebsocket socket) : base(client, socket)
            {
            }
        }
    }
}
