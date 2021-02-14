using AlterDice.Net;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AlterDice.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var ad = new AlterDiceClient("", "");
         

            var balances = await ad.GetBalancesAsync();
            foreach (var b in balances.Data.Where(c => c.Balance > 0))
            {
                Console.WriteLine($"{b.Currency.ShortName}: {b.Balance}");
            }
           

            var orders = await ad.GetActiveOrdersAsync();
            foreach(var o in orders.Data)
                Console.WriteLine($"{o.Symbol}: {o.Price} {o.Quantity} {o.OrderSide} {o.CreatedAt}");
            Console.WriteLine();

            //var s = new AlterDiceSocketClient("ad", new CryptoExchange.Net.Objects.SocketClientOptions("https://socket.alterdice.com"), new AlterDiceAuthenticationProvider(new CryptoExchange.Net.Authentication.ApiCredentials("42","42")));
            //s.OnOrderBookUpdate += S_OnOrderBookUpdate;
            //await s.SubscribeToBook("EEXBTC");

            /*{"status":true,"message":"Success send request to create order","data":{"id":3069958826},"token":"df14f83975482f4786e4415b8c721b7b387ddec2adaaf6bb17cb8ec5db60f212"}*/

            //var cancel = await ad.CancelOrderAsync(3069958826);


            //var book = await ad.GetOrderBookAsync("EEXBTC");
            //var lockSpread = book.Data.Bids.Max(c => c.Price) + 0.00000001m;
            //var order = await ad.PlaceOrderAsync(new Net.Objects.AlterDicePlaceOrderRequest() 
            //{ 
            //        OrderSide= Net.Objects.AlterDiceOrderSide.Sell,
            //        OrderType=Net.Objects.AlterDiceOrderType.Limit,
            //        Price=lockSpread,
            //        Quantity=0.001m,
            //        Symbol="EEXBTC"
            //});
            //if(order)
            //{
            //    Console.WriteLine($"Order for sell 0.001m EEX by {lockSpread} BTC placed with id {order.Data}");
            //}
            Console.WriteLine();
            Console.ReadLine();
        }

        private static void S_OnOrderBookUpdate(object obj)
        {
            Console.WriteLine(obj);
        }
    }
}
