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
            var ad = new AlterDiceClient();

            var book = new AlterDiceSymbolOrderBook("BTCUSDT", ad, new AlterDiceOrderBookOptions("asd", 1200));
            book.OnOrderBookUpdate += Book_OnOrderBookUpdate;
            var t = book.StartAsync();
            Console.WriteLine(t);
            Console.ReadLine();
        }

        private static void Book_OnOrderBookUpdate((System.Collections.Generic.IEnumerable<CryptoExchange.Net.Interfaces.ISymbolOrderBookEntry> Bids, System.Collections.Generic.IEnumerable<CryptoExchange.Net.Interfaces.ISymbolOrderBookEntry> Asks) obj)
        {
            Console.WriteLine($"{DateTime.UtcNow} Updated ob: {obj.Asks.Count()} {obj.Bids.Count()} ");
        }
    }
}
