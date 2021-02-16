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
using CryptoExchange.Net.Logging;

namespace AlterDice.Net
{
    public class AlterDiceSymbolOrderBook : SymbolOrderBook
    {
        private readonly System.Timers.Timer _timer;
        private readonly AlterDiceSocketClient _socket;
        private readonly AlterDiceClient _apiClient;
        private readonly bool _shouldUseApi;
        private readonly int _symbolId;
        public AlterDiceSymbolOrderBook(string symbol, AlterDiceClient client, AlterDiceOrderBookOptions opts) : this(symbol, opts)
        {
            _apiClient = client;
        }
        public AlterDiceSymbolOrderBook(string symbol, AlterDiceOrderBookOptions options) : base(symbol, options)
        {
            _symbolId = options.SymbolId;
            _apiClient = new AlterDiceClient();
            if (options.SymbolId == 0 && options.Timeout.HasValue)
            {
                _shouldUseApi = true;
                _timer = new System.Timers.Timer(options.Timeout.Value);
                _timer.Elapsed += T_Elapsed;
            }

            _socket = new AlterDiceSocketClient("asd", new SocketClientOptions("https://socket.alterdice.com") 
            {  
                LogVerbosity=CryptoExchange.Net.Logging.LogVerbosity.Debug,
                LogWriters = new System.Collections.Generic.List<System.IO.TextWriter>() { new ThreadSafeFileWriter("socket.txt"), new DebugTextWriter() },
            }, 
            new AlterDiceAuthenticationProvider(new CryptoExchange.Net.Authentication.ApiCredentials("42", "42")));
            _socket.OnOrderBookUpdate += _socket_OnOrderBookUpdate1;
        }

        private void _socket_OnOrderBookUpdate1(object obj)
        {

        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            Task.Run(async () => await GetAndSetBook());
        }

        private async Task<CallResult<bool>> GetAndSetBook()
        {
            try
            {
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

            return await GetAndSetBook();
        }

        protected override async Task<CallResult<UpdateSubscription>> DoStart()
        {
            //_socket.OnOrderBookUpdate += _socket_OnOrderBookUpdate;
            WebsocketFactory wf = new WebsocketFactory();
            await GetAndSetBook();
            if (_shouldUseApi)
            {
                _timer.Start();
            }
            else
            {
                await _socket.SubscribeToBook(_symbolId);
            }
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
