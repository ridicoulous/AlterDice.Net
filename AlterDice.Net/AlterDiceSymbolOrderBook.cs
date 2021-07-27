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
using AlterDice.Net.Objects.Socket;
using System.Linq;
using AlterDice.Net.Objects;

namespace AlterDice.Net
{
    public class AlterDiceSymbolOrderBook : SymbolOrderBook
    {
        private readonly System.Timers.Timer _timer;
        private readonly AlterDiceSocketClient _socket;
        private readonly AlterDiceClient _apiClient;
        private readonly bool _shouldUseApi;
        private readonly int _symbolId;
        /// <summary>
        /// The last used id
        /// </summary>
        protected static long lastId;
        /// <summary>
        /// Lock for id generating
        /// </summary>
        protected static object idLock = new object();
        /// <summary>
        /// Last is used
        /// </summary>
        public static long LastId => lastId;

        /// <summary>
        /// Generate a unique id
        /// </summary>
        /// <returns></returns>
        protected long NextId()
        {
            lock (idLock)
            {
                lastId++;
                return lastId;
            }
        }
        public AlterDiceSymbolOrderBook(string symbol, AlterDiceClient client, AlterDiceOrderBookOptions opts) : this(symbol, opts)
        {
            _apiClient = client;
        }
        public AlterDiceSymbolOrderBook(string symbol, AlterDiceOrderBookOptions options) : base(symbol, options)
        {
            _symbolId = options.SymbolId;
            _apiClient = new AlterDiceClient();
            _shouldUseApi = options.SymbolId == 0;

            _timer = new System.Timers.Timer(options.Timeout ?? 10000);
            _timer.Elapsed += T_Elapsed;

            _socket = new AlterDiceSocketClient("AlterDiceSocketBook", new SocketClientOptions("https://socket.alterdice.com")
            {
                LogVerbosity = CryptoExchange.Net.Logging.LogVerbosity.Debug,
                LogWriters = new System.Collections.Generic.List<System.IO.TextWriter>() { new DebugTextWriter() },
            },
            new AlterDiceAuthenticationProvider(new CryptoExchange.Net.Authentication.ApiCredentials("42", "42")));

        }

        private void _socket_OnOrderBookUpdate1(AlterDiceSocketOrderBookUpdateEvent data)
        {
            if (this.BidCount==0||this.AskCount==0)
            {
                GetAndSetBook().GetAwaiter().GetResult();
            }
            var bids = data.Data.Bids.Values.Select(c => new AlterDiceOrderBookEntry() { Count = (int)c.Count, Price = c.Rate / 1e8m, Quantity = c.Volume / 1e8m }).ToList();
            var asks = data.Data.Asks.Values.Select(c => new AlterDiceOrderBookEntry() { Count = (int)c.Count, Price = c.Rate / 1e8m, Quantity = c.Volume / 1e8m }).ToList();
            UpdateOrderBook(DateTime.UtcNow.Ticks, bids, asks);
        }

        private void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            Task.Run(async () => await GetAndSetBook());

            if (!_shouldUseApi)
            {
                Task.Run(async () => await _socket.Send("ping"));
            }
            
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
            asks.Clear();
            bids.Clear();
            _socket.Dispose();
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
            _timer.Start();

            if (!_shouldUseApi)
            {
                _socket.OnOrderBookUpdate += _socket_OnOrderBookUpdate1;
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
