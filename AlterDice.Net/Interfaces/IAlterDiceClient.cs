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
        Task<WebCallResult<AlterDiceOrder>> CancelOrderAsync(long orderId, CancellationToken ct = default);
        WebCallResult<AlterDiceOrder> CancelOrder(long orderId);
        Task<WebCallResult<List<AlterDiceOrder>>> GetOrdersHistoryAsync(CancellationToken ct = default);
        WebCallResult<List<AlterDiceOrder>> GetOrdersHistory();
        Task<WebCallResult<List<AlterDiceBalance>>> GetBalancesAsync(CancellationToken ct = default);
        WebCallResult<List<AlterDiceBalance>> GetBalances();


    }
}
