using AlterDice.Net.Helpers;
using AlterDice.Net.Interfaces;
using AlterDice.Net.Objects;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.ExchangeInterfaces;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private const string PublicTradesHistoryUrl = "public/trades?pair={}";
        private const string TickerUrl = "public/ticker?pair={}";
        private const string TickersUrl = "public/tickers";
        private const string SymbolsUrl = "public/symbols";
        
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
#warning check api docs at <see cref="https://docs.alterdice.com/#api-Private_API-Number_Formatting">
            if (request)
            {
                foreach(var o in request.Data.Response.Orders)
                {
                    o.Price *= 1e8m;
                    o.Quantity *= 1e8m;
                    o.QuantityDone *= 1e8m;
                    o.QuoteQuantityFilled*= 1e8m;
                    o.QuoteQuantity*= 1e8m;
                }
            }
            return new WebCallResult<List<AlterDiceOrder>>(request.ResponseStatusCode, request.ResponseHeaders, request.Data?.Response?.Orders, request.Error);
        }

        public WebCallResult<List<AlterDiceOrder>> GetActiveOrders() => GetActiveOrdersAsync().Result;

        public async Task<WebCallResult<AlterDiceOrder>> GetOrderAsync(long orderId, CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDiceGetOrderResponse>(GetUrl(GetOrderUrl), HttpMethod.Post, ct, new AlterDiceGetOrderRequest(orderId).AsDictionary(), true, false);
            return Map<AlterDiceGetOrderResponse, AlterDiceOrder>(request);
        }
        public WebCallResult<AlterDiceOrder> GetOrder(long orderId) => GetOrderAsync(orderId).Result;

        public async Task<WebCallResult<AlterDiceGetOrdersResult>> GetOrdersHistoryAsync(int page = 1, int limit = 2000, CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDiceGetOrdersResponse>(GetUrl(OrdersHistoryUrl), HttpMethod.Post, ct, new AlterDicePagedAuthenticatedRequest(page, limit).AsDictionary(), true, false);

            return new WebCallResult<AlterDiceGetOrdersResult>(request.ResponseStatusCode, request.ResponseHeaders, request ? new AlterDiceGetOrdersResult(request.Data?.Response?.Pagination, request.Data?.Response?.Orders) : null, request.Error);
        }

        public WebCallResult<AlterDiceGetOrdersResult> GetOrdersHistory(int page = 1, int limit = 2000) => GetOrdersHistoryAsync(page, limit).Result;

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
            return baseAsset + quoteAsset;
        }

        async Task<WebCallResult<IEnumerable<ICommonSymbol>>> IExchangeClient.GetSymbolsAsync()
        {
            return WebCallResult<IEnumerable<ICommonSymbol>>.CreateFrom(await GetSymbolsAsync());
        }

        async Task<WebCallResult<IEnumerable<ICommonTicker>>> IExchangeClient.GetTickersAsync()
        {
            return WebCallResult<IEnumerable<ICommonTicker>>.CreateFrom(await GetTickersAsync());
        }

        async Task<WebCallResult<ICommonTicker>> IExchangeClient.GetTickerAsync(string symbol)
        {
            return WebCallResult<ICommonTicker>.CreateFrom(await GetTickerAsync(symbol));
        }

        public Task<WebCallResult<IEnumerable<ICommonKline>>> GetKlinesAsync(string symbol, TimeSpan timespan, DateTime? startTime = null, DateTime? endTime = null, int? limit = null)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<ICommonOrderBook>> IExchangeClient.GetOrderBookAsync(string symbol)
        {
            var book = await GetOrderBookAsync(symbol);
            return WebCallResult<ICommonOrderBook>.CreateFrom(book);
        }

        public async Task<WebCallResult<List<AlterDicePublicTrade>>> GetLastPublicTradesAsync(string symbol, CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDicePublicTradeResponse>(GetUrl(FillPathParameter(PublicTradesHistoryUrl, symbol)), HttpMethod.Get, ct, null, false, false);
            return new WebCallResult<List<AlterDicePublicTrade>>(request.ResponseStatusCode, request.ResponseHeaders, request.Data?.Response, request.Error);
        }

        public WebCallResult<List<AlterDicePublicTrade>> GetLastPublicTrades(string symbol) => GetLastPublicTradesAsync(symbol).Result;
        public async Task<WebCallResult<IEnumerable<ICommonRecentTrade>>> GetRecentTradesAsync(string symbol)
        {
            return WebCallResult<IEnumerable<ICommonRecentTrade>>.CreateFrom(await GetLastPublicTradesAsync(symbol));
        }

        public async Task<WebCallResult<ICommonOrderId>> PlaceOrderAsync(string symbol, IExchangeClient.OrderSide side, IExchangeClient.OrderType type, decimal quantity, decimal? price = null, string accountId = null)
        {
            var placeOrderRequest = new AlterDicePlaceOrderRequest()
            {
                OrderSide = side == IExchangeClient.OrderSide.Buy ? AlterDiceOrderSide.Buy : AlterDiceOrderSide.Sell,
                OrderType = type == IExchangeClient.OrderType.Limit ? AlterDiceOrderType.Limit : AlterDiceOrderType.Market,
                Price = price,
                Quantity = quantity,
                Symbol = symbol
            };
            var request = await PlaceOrderAsync(placeOrderRequest);
            if (request)
                return new WebCallResult<ICommonOrderId>(request.ResponseStatusCode, request.ResponseHeaders, new AlterDiceOrderResponse() { OrderId = request.Data }, request.Error);

            return WebCallResult<ICommonOrderId>.CreateErrorResult(request.Error);
        }

        public async Task<WebCallResult<ICommonOrder>> GetOrderAsync(string orderId, string symbol = null)
        {
            long id;
            if (long.TryParse(orderId, out id))
            {
                var order = await GetOrderAsync(id);
                return WebCallResult<ICommonOrder>.CreateFrom(order);
            }
            return WebCallResult<ICommonOrder>.CreateErrorResult(new ServerError($"Can not parse orderId {orderId}"));
        }

        public Task<WebCallResult<IEnumerable<ICommonTrade>>> GetTradesAsync(string orderId, string symbol = null)
        {
            throw new NotImplementedException();
        }

        public async Task<WebCallResult<IEnumerable<ICommonOrder>>> GetOpenOrdersAsync(string symbol = null)
        {
            var orders = await GetActiveOrdersAsync();

            if (!orders)
            {
                return WebCallResult<IEnumerable<ICommonOrder>>.CreateErrorResult(orders.Error);
            }
            var result = orders.Data;
            if (!String.IsNullOrEmpty(symbol))
            {
                result = result.Where(c => c.Symbol == symbol).ToList();
            }
            return new WebCallResult<IEnumerable<ICommonOrder>>(orders.ResponseStatusCode, orders.ResponseHeaders, result, null);
        }

        public async Task<WebCallResult<IEnumerable<ICommonOrder>>> GetClosedOrdersAsync(string symbol = null)
        {
            var orders = await GetAllOrdersHistoryAsync();

            if (!orders)
            {
                return WebCallResult<IEnumerable<ICommonOrder>>.CreateErrorResult(orders.Error);
            }
            var result = orders.Data;
            if (!String.IsNullOrEmpty(symbol))
            {
                result = result.Where(c => c.Symbol == symbol).ToList();
            }
            return new WebCallResult<IEnumerable<ICommonOrder>>(orders.ResponseStatusCode, orders.ResponseHeaders, result, null);
        }

        public async Task<WebCallResult<ICommonOrderId>> CancelOrderAsync(string orderId, string symbol = null)
        {
            var result = await CancelOrderAsync(long.Parse(orderId));
            if (result || (!result && result.Error.Message.Contains("Order not found")))
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

        public async Task<WebCallResult<List<AlterDiceOrder>>> GetAllOrdersHistoryAsync(int? limit = null, CancellationToken ct = default)
        {
            var result = new List<AlterDiceOrder>();
            int startPage = 1;
            int endPage = 10;
            Error? error = null;
            while (startPage <= endPage)
            {
                var orders = await GetOrdersHistoryAsync(startPage);
                if (orders)
                {
                    if (result.Select(i => i.Id).Intersect(orders.Data.Orders.Select(f => f.Id)).Any())
                    {
                        log.Write(CryptoExchange.Net.Logging.LogVerbosity.Error, "Obtained the same orders");
                        break;
                    }
                    result.AddRange(orders.Data.Orders);
                    if (limit.HasValue && limit.Value <= result.Count)
                    {
                        break;
                    }
                    startPage = orders.Data.Pagination.CurrentPage + 1;
                    endPage = orders.Data.Pagination.TotalPagesCount;
                    if (!orders.Data.Orders.Any())
                    {
                        break;
                    }
                }
                else
                {
                    error = orders.Error;
                    break;
                }
            }
            return new WebCallResult<List<AlterDiceOrder>>(System.Net.HttpStatusCode.OK, null, result, error);
        }

        public WebCallResult<List<AlterDiceOrder>> GetAllOrdersHistory(int? limit = null) => GetAllOrdersHistoryAsync(limit).Result;

        public async Task<WebCallResult<List<AlterDiceSymbol>>> GetSymbolsAsync(CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDiceBaseResponse<List<AlterDiceSymbol>>>(GetUrl(SymbolsUrl), HttpMethod.Get, ct, null, false, false);
            return new WebCallResult<List<AlterDiceSymbol>>(request.ResponseStatusCode, request.ResponseHeaders, request.Data?.Response, request.Error);
        }

        public WebCallResult<List<AlterDiceSymbol>> GetSymbols() => GetSymbolsAsync().Result;
        public async Task<WebCallResult<AlterDiceTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDiceBaseResponse<AlterDiceTicker>>(GetUrl(FillPathParameter(TickerUrl, symbol)), HttpMethod.Get, ct, null, false, false);
            return new WebCallResult<AlterDiceTicker>(request.ResponseStatusCode, request.ResponseHeaders, request.Data?.Response, request.Error);
        }

        public WebCallResult<AlterDiceTicker> GetTicker(string symbol) => GetTickerAsync(symbol).Result;
        public async Task<WebCallResult<IEnumerable<AlterDiceTicker>>> GetTickersAsync(CancellationToken ct = default)
        {
            var request = await SendRequest<AlterDiceBaseResponse<IEnumerable<AlterDiceTicker>>>(GetUrl(TickersUrl), HttpMethod.Get, ct, null, false, false);
            return new WebCallResult<IEnumerable<AlterDiceTicker>>(request.ResponseStatusCode, request.ResponseHeaders, request.Data?.Response, request.Error);
        }

        public WebCallResult<IEnumerable<AlterDiceTicker>> GetTickers() => GetTickersAsync().Result;
    }
}
