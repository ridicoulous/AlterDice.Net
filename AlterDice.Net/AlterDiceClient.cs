using AlterDice.Net.Helpers;
using AlterDice.Net.Interfaces;
using AlterDice.Net.Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.ExchangeInterfaces;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AlterDice.Net
{
    public class AlterDiceClient : RestClient, IAlterDiceClient, IExchangeClient
    {
        #region Endpoints
        private const string LoginUrl = "login";
        private const string OrderBookUrl = "public/book?pair={}";
        private const string PlaceOrderUrl = "private/create-order";
        private const string GetActiveOrdersUrl = "private/orders";
        private const string GetOrderUrl = "private/get-order";
        private const string CancelOrderUrl = "private/delete-order";
        private const string OrdersHistoryUrl = "private/history";
        private const string BalancesUrl = "private/balances";

        #endregion
        private readonly string LoginEmail, Password;

        private static AlterDiceClientOptions defaultOptions = new AlterDiceClientOptions();

        public AlterDiceClient() : this("AlterDiceClient", defaultOptions, null)
        {
        }
        public AlterDiceClient(string login, string pass) : this("AlterDiceClient", new AlterDiceClientOptions(login, pass), null)
        {
        }
        public AlterDiceClient(AlterDiceClientOptions opts) : this("AlterDiceClient", opts, null)
        {
        }

        public AlterDiceClient(AlterDiceClientOptions exchangeOptions, AlterDiceAuthenticationProvider authenticationProvider) : this("AlterDiceClient", exchangeOptions, authenticationProvider)
        {
        }
        public AlterDiceClient(string clientName, AuthenticationProvider authenticationProvider) : this(clientName, defaultOptions, authenticationProvider)
        {
        }
        public AlterDiceClient(string clientName, AlterDiceClientOptions exchangeOptions, AuthenticationProvider authenticationProvider) : base(clientName, exchangeOptions, authenticationProvider)
        {
            LoginEmail = exchangeOptions.Login;
            Password = exchangeOptions.Password;
            if (!String.IsNullOrEmpty(LoginEmail) && !String.IsNullOrEmpty(Password) && authenticationProvider == null)
            {
                LoginAsync().GetAwaiter().GetResult();
            }
        }
        public AuthenticationProvider GetAuth()
        {
            return this.authProvider;
        }
        private async Task LoginAsync(CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDiceLoginResponse>(GetUrl(LoginUrl), HttpMethod.Post, ct, new AlterDiceLoginRequest() { Email = this.LoginEmail, Password = Password }.AsDictionary(), false, false);
            if (request)
            {
                this.authProvider = new AlterDiceAuthenticationProvider(new ApiCredentials(request.Data.Token, request.Data.Response.Secret));
                this.log.Write(CryptoExchange.Net.Logging.LogVerbosity.Debug, $"Client was authenticated");
            }
            else
            {
                throw new Exception("Can not login with provided credentials");
            }
        }


        private Uri GetUrl(string endpoint)
        {
            return new Uri(BaseAddress + endpoint);
        }

        public async Task<WebCallResult<AlterDiceOrderBook>> GetOrderBookAsync(string pair, CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDiceOrderBookResponse>(GetUrl(FillPathParameter(OrderBookUrl, pair)), HttpMethod.Get, ct);
            return Map<AlterDiceOrderBookResponse, AlterDiceOrderBook>(request);
        }
        private WebCallResult<TOut> Map<TIn, TOut>(WebCallResult<TIn> request) where TIn : AlterDiceBaseResponse<TOut>
        {
            if (request)
            {
                return new WebCallResult<TOut>(request.ResponseStatusCode, request.ResponseHeaders, request.Data.Response, request.Error);
            }
            else
            {
                return new WebCallResult<TOut>(request.ResponseStatusCode, request.ResponseHeaders, default, request.Error);
            }
        }
        public WebCallResult<AlterDiceOrderBook> GetOrderBook(string pair) => GetOrderBookAsync(pair).Result;

        public async Task<WebCallResult<long>> PlaceOrderAsync(AlterDicePlaceOrderRequest placeOrderRequest, CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDicePlaceOrderResponse>(GetUrl(PlaceOrderUrl), HttpMethod.Post, ct, placeOrderRequest.AsDictionary(), true, false);
            return new WebCallResult<long>(request.ResponseStatusCode, request.ResponseHeaders, request.Data?.Response?.OrderId ?? 0, request.Error);
        }
        public WebCallResult<long> PlaceOrder(AlterDicePlaceOrderRequest placeOrderRequest) => PlaceOrderAsync(placeOrderRequest).Result;
        public async Task<WebCallResult<List<AlterDiceOrder>>> GetActiveOrdersAsync(CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDiceGetOrdersResponse>(GetUrl(GetActiveOrdersUrl), HttpMethod.Post, ct, new AlterDiceAuthenticatedRequest().AsDictionary(), true, false);
            return new WebCallResult<List<AlterDiceOrder>>(request.ResponseStatusCode, request.ResponseHeaders, request.Data?.Response?.Orders, request.Error);
        }

        public WebCallResult<List<AlterDiceOrder>> GetActiveOrders() => GetActiveOrdersAsync().Result;

        public async Task<WebCallResult<AlterDiceOrder>> GetOrderAsync(long orderId, CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDiceGetOrderResponse>(GetUrl(GetOrderUrl), HttpMethod.Post, ct, new AlterDiceGetOrderRequest(orderId).AsDictionary(), true, false);
            return Map<AlterDiceGetOrderResponse, AlterDiceOrder>(request);
        }
        public WebCallResult<AlterDiceOrder> GetOrder(long orderId) => GetOrderAsync(orderId).Result;

        public async Task<WebCallResult<List<AlterDiceOrder>>> GetOrdersHistoryAsync(CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDiceGetOrdersResponse>(GetUrl(OrdersHistoryUrl), HttpMethod.Post, ct, new AlterDiceAuthenticatedRequest().AsDictionary(), true, false);
            return new WebCallResult<List<AlterDiceOrder>>(request.ResponseStatusCode, request.ResponseHeaders, request.Data?.Response?.Orders, request.Error);
        }

        public WebCallResult<List<AlterDiceOrder>> GetOrdersHistory() => GetOrdersHistoryAsync().Result;

        public async Task<WebCallResult<bool>> CancelOrderAsync(long orderId, CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDiceGetOrderResponse>(GetUrl(CancelOrderUrl), HttpMethod.Post, ct, new AlterDiceGetOrderRequest(orderId).AsDictionary(), true, false);
            return new WebCallResult<bool>(request.ResponseStatusCode, request.ResponseHeaders, request, request.Error);
        }

        public WebCallResult<bool> CancelOrder(long orderId) => CancelOrderAsync(orderId).Result;

        public async Task<WebCallResult<List<AlterDiceBalance>>> GetBalancesAsync(CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDiceBalancesResponse>(GetUrl(BalancesUrl), HttpMethod.Post, ct, new AlterDiceAuthenticatedRequest().AsDictionary(), true, false);
            return new WebCallResult<List<AlterDiceBalance>>(request.ResponseStatusCode, request.ResponseHeaders, request.Data?.Response?.Result, request.Error);
        }

        public WebCallResult<List<AlterDiceBalance>> GetBalances() => GetBalancesAsync().Result;

        public string GetSymbolName(string baseAsset, string quoteAsset)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<ICommonSymbol>>> GetSymbolsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<ICommonTicker>>> GetTickersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<ICommonTicker>> GetTickerAsync(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<ICommonKline>>> GetKlinesAsync(string symbol, TimeSpan timespan, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<ICommonOrderBook>> IExchangeClient.GetOrderBookAsync(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<ICommonRecentTrade>>> GetRecentTradesAsync(string symbol)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<ICommonOrderId>> IExchangeClient.PlaceOrderAsync(string symbol, IExchangeClient.OrderSide side, IExchangeClient.OrderType type, decimal quantity, decimal? price = null, string accountId = null)
        {
            var placeOrderRequest = new AlterDicePlaceOrderRequest()
            {
                OrderSide = side == IExchangeClient.OrderSide.Buy ? AlterDiceOrderSide.Buy : AlterDiceOrderSide.Sell,
                OrderType = type == IExchangeClient.OrderType.Limit ? AlterDiceOrderType.Limit : AlterDiceOrderType.Market,
                Price = price,
                Quantity = quantity,
                Symbol = symbol
            };
           // var request = await SendRequest<AlterDicePlaceOrderResponse>(GetUrl(PlaceOrderUrl), HttpMethod.Post, default, placeOrderRequest.AsDictionary(), true, false);
            var request = await PlaceOrderAsync(placeOrderRequest);
            if(request)
                return new WebCallResult<ICommonOrderId>(request.ResponseStatusCode, request.ResponseHeaders, new AlterDiceOrderResponse() { OrderId=request.Data}, request.Error);
                return new WebCallResult<ICommonOrderId>(request.ResponseStatusCode, request.ResponseHeaders, null, request.Error);

        }

        public Task<WebCallResult<ICommonOrder>> GetOrderAsync(string orderId, string symbol = null)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<ICommonTrade>>> GetTradesAsync(string orderId, string symbol = null)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<ICommonOrder>>> GetOpenOrdersAsync(string symbol = null)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<ICommonOrder>>> GetClosedOrdersAsync(string symbol = null)
        {
            throw new NotImplementedException();
        }

        public async Task<WebCallResult<ICommonOrderId>> CancelOrderAsync(string orderId, string symbol = null)
        {
            var result = await CancelOrderAsync(long.Parse(orderId));
            if (result)
            {
                return new WebCallResult<ICommonOrderId>(result.ResponseStatusCode, result.ResponseHeaders, new AlterDiceOrderResponse(long.Parse(orderId)), null);
            }
            return new WebCallResult<ICommonOrderId>(result.ResponseStatusCode, result.ResponseHeaders, null, result.Error);

        }

        async Task<WebCallResult<IEnumerable<ICommonBalance>>> IExchangeClient.GetBalancesAsync(string accountId = null)
        {
            var request = await SendRequest<AlterDiceBalancesResponse>(GetUrl(BalancesUrl), HttpMethod.Post, default, new AlterDiceAuthenticatedRequest().AsDictionary(), true, false);
            return new WebCallResult<IEnumerable<ICommonBalance>>(request.ResponseStatusCode, request.ResponseHeaders, request.Data?.Response?.Result, request.Error);
        }

    }
}
