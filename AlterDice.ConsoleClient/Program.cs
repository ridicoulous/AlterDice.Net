using AlterDice.Net;
using System;
using System.Threading.Tasks;

namespace AlterDice.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var ad = new AlterDiceClient("email", "password");
            var balances = await ad.GetBalancesAsync();
            var tt = await ad.CancelOrderAsync(42);
            Console.WriteLine();
        }
    }
}
