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
            
            Console.ReadLine();
        }

        
    }
}
