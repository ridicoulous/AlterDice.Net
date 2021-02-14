using AlterDice.Net.Interfaces;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
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

        public AlterDiceSocketClient(string clientName, SocketClientOptions exchangeOptions, AuthenticationProvider authenticationProvider):base(clientName,exchangeOptions,authenticationProvider)
        {
            //_socketIo = IO.Socket("https://socket.alterdice.com");
            //_socketIo.Connect();
            _socketIo = new SocketIO("https://socket.alterdice.com/");
            _socketIo.OnConnected += _socketIo_OnConnected;
            _socketIo.OnError += _socketIo_OnError;
            _socketIo.OnReceivedEvent += _socketIo_OnReceivedEvent;
            _socketIo.OnDisconnected += _socketIo_OnDisconnected;
            
            _socketIo.On("hi", response =>
            {
                string text = response.GetValue<string>();
                Console.WriteLine(text);
            });
            _socketIo.OnConnected += async (sender, e) =>
            {
                await _socketIo.EmitAsync("hi", ".net core");
                Console.WriteLine(e.ToString());
            };
            _socketIo.On("message", (data) =>
            {
                Console.WriteLine(data);
                OnOrderBookUpdate?.Invoke(data);
            });

            _socketIo.ConnectAsync().GetAwaiter().GetResult();
        }

        private void _socketIo_OnDisconnected(object sender, string e)
        {
            throw new NotImplementedException();
        }

        private void _socketIo_OnReceivedEvent(object sender, SocketIOClient.EventArguments.ReceivedEventArgs e)
        {
            Console.WriteLine($"Received {e.Event}: {e.Response}");
        }

        private void _socketIo_OnError(object sender, string e)
        {
            throw new NotImplementedException();
        }

        public event Action<object> OnOrderBookUpdate;

        public async Task SubscribeToBook(string pair)
        {
           
            //_socketIo.Socket.SendMessageAsync("{type: 'book', event: 'book_7871'}");
         await   _socketIo.EmitAsync("subscribe", "{\"type\": \"book\", \"event\": \"book_3051\"}");
          //  _socketIo.Socket.SendMessageAsync("");
        }

        private void _socketIo_OnConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Connected");
        }

        protected override async Task<CallResult<bool>> AuthenticateSocket(SocketConnection s)
        {
            return new CallResult<bool>(false,null);
        }

        protected override bool HandleQueryResponse<T>(SocketConnection s, object request, JToken data,  out CallResult<T> callResult)
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
