using AlterDice.Net.Interfaces;
using AlterDice.Net.Objects.Socket;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace AlterDice.Net
{
    public class AlterDiceSocketClient : SocketClient, IAlterDiceSocketClient
    {
        private readonly SocketIO _socketIo;

        public AlterDiceSocketClient(string clientName, SocketClientOptions exchangeOptions, AuthenticationProvider authenticationProvider) : base(clientName, exchangeOptions, authenticationProvider)
        {

            //_socketIo = IO.Socket("https://socket.alterdice.com");
            //_socketIo.Connect();
            _socketIo = new SocketIO("https://socket.alterdice.com/",new SocketIOOptions() { EIO=3,ConnectionTimeout=TimeSpan.FromSeconds(20),Reconnection=true,});
            _socketIo.OnPong += _socketIo_OnPong;
            _socketIo.OnConnected += _socketIo_OnConnected;
            _socketIo.OnError += _socketIo_OnError;
            
            _socketIo.OnReceivedEvent += _socketIo_OnReceivedEvent;
     
            _socketIo.OnDisconnected += _socketIo_OnDisconnected;

          
            _socketIo.OnConnected += async (sender, e) =>
            {
                //await _socketIo.EmitAsync("hi", ".net core");
                Console.WriteLine($"{DateTime.UtcNow}: connected");
            };
            _socketIo.On("message", (data) =>
            {               
                var t = JsonConvert.DeserializeObject<List<AlterDiceSocketOrderBookUpdateEvent>>(data.GetValue(0).ToString());
                OnOrderBookUpdate?.Invoke(t[0]);
            });
            _socketIo.ConnectAsync().GetAwaiter().GetResult();
            
        }

        private void _socketIo_OnPong(object sender, TimeSpan e)
        {
            Console.WriteLine($"{DateTime.UtcNow}: pong recieved");

        }
        public async Task Send(string data)
        {
           // Console.WriteLine($"{DateTime.UtcNow}: Sending {data}");
            await _socketIo.EmitAsync(data);
        }
        private void _socketIo_OnDisconnected(object sender, string e)
        {
            Console.WriteLine($"{DateTime.UtcNow} : disconnected");
            log.Write(CryptoExchange.Net.Logging.LogVerbosity.Debug, $"Socket.io client disconnected: {e}");
            //_socketIo.ConnectAsync().GetAwaiter().GetResult();
        }

        private void _socketIo_OnReceivedEvent(object sender, SocketIOClient.EventArguments.ReceivedEventArgs e)
        {
            log.Write(CryptoExchange.Net.Logging.LogVerbosity.Debug, $"Socket.io Received {e.Event}: {e.Response}");
        }

        private void _socketIo_OnError(object sender, string e)
        {
            Console.WriteLine($"{DateTime.UtcNow}: err {e}");

            log.Write(CryptoExchange.Net.Logging.LogVerbosity.Debug, $"Socket.io client error: {e}");
        }

        public event Action<AlterDiceSocketOrderBookUpdateEvent> OnOrderBookUpdate;
      
        public async Task SubscribeToBook(int symbolId)
        {
            log.Write(CryptoExchange.Net.Logging.LogVerbosity.Debug, $"Subscribing to bookId {symbolId}");

            //await _socketIo.EmitAsync("subscribe", new SubscribeRequest("short_book",symbolId));          
            await _socketIo.EmitAsync("subscribe", $"short_book_{symbolId}");
        }

        private void _socketIo_OnConnected(object sender, EventArgs e)
        {
            log.Write(CryptoExchange.Net.Logging.LogVerbosity.Debug, "Socket.io client was connected");
        }

        protected override async Task<CallResult<bool>> AuthenticateSocket(SocketConnection s)
        {
            return new CallResult<bool>(false, null);
        }

        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken message, out CallResult<object> callResult)
        {
            throw new NotImplementedException();
        }

        protected override bool MessageMatchesHandler(JToken message, object request)
        {
            throw new NotImplementedException();
        }

        protected override bool MessageMatchesHandler(JToken message, string identifier)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> Unsubscribe(SocketConnection connection, SocketSubscription s)
        {
            throw new NotImplementedException();
        }
    }
}
