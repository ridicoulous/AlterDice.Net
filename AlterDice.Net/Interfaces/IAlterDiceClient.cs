using AlterDice.Net.Objects;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlterDice.Net.Interfaces
{
    public interface IAlterDiceClient
    {
        Task<WebCallResult<AlterDiceOrderBook>> GetOrderBookAsync(string pair, CancellationToken ct = default);
        WebCallResult<AlterDiceOrderBook> GetOrderBook(string pair);

        Task<WebCallResult<long>> PlaceOrderAsync(AlterDicePlaceOrderRequest placeOrderRequest, CancellationToken ct=default);
        WebCallResult<long> PlaceOrder(AlterDicePlaceOrderRequest placeOrderRequest);
        Task<WebCallResult<List<AlterDiceOrder>>> GetActiveOrdersAsync( CancellationToken ct = default);
        WebCallResult<List<AlterDiceOrder>> GetActiveOrders();
        Task<WebCallResult<AlterDiceOrder>> GetOrderAsync(long orderId, CancellationToken ct = default);
        WebCallResult<AlterDiceOrder> GetOrder(long orderId);
        Task<WebCallResult<bool>> CancelOrderAsync(long orderId, CancellationToken ct = default);
        WebCallResult<bool> CancelOrder(long orderId);
        Task<WebCallResult<AlterDiceGetOrdersResult>> GetOrdersHistoryAsync(int page=1, int limit = 2000, CancellationToken ct = default);
        WebCallResult<AlterDiceGetOrdersResult> GetOrdersHistory(int page=1, int limit=2000);

        WebCallResult<List<AlterDiceOrder>> GetAllOrdersHistory(int? limit = null);
        Task<WebCallResult<List<AlterDiceOrder>>> GetAllOrdersHistoryAsync(int? limit = null,CancellationToken ct = default);

        Task<WebCallResult<List<AlterDiceBalance>>> GetBalancesAsync(CancellationToken ct = default);
        WebCallResult<List<AlterDiceBalance>> GetBalances();

        /// <summary>
        /// returns only 25 last trades fo the specified pair
        /// </summary>
        /// <returns></returns>
        Task<WebCallResult<List<AlterDicePublicTrade>>> GetLastPublicTradesAsync(string pair, CancellationToken ct = default);

        Task<WebCallResult<List<AlterDiceSymbol>>> GetSymbolsAsync(CancellationToken ct = default);
        WebCallResult<List<AlterDiceSymbol>> GetSymbols();
        Task<WebCallResult<AlterDiceTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);
        WebCallResult<AlterDiceTicker> GetTicker(string symbol);
        Task<WebCallResult<IEnumerable<AlterDiceTicker>>> GetTickersAsync(CancellationToken ct = default);
        WebCallResult<IEnumerable<AlterDiceTicker>> GetTickers();
    }
}
