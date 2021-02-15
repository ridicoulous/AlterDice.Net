using AlterDice.Net;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AlterDice.ConsoleClient
{
    class Program
    {
        static DateTime _lastUpdate = DateTime.UtcNow;
        static async Task Main(string[] args)
        {
            var ad = new AlterDiceClient("", "");
            var b = await ad.GetBalancesAsync();
            Console.WriteLine(t);
            Console.ReadLine();
        }


    }
}
