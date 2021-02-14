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
      
          
            Console.ReadLine();
        }

    }
}
