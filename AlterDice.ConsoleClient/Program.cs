using AlterDice.Net;
using CryptoExchange.Net.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AlterDice.ConsoleClient
{
    class Program
    {
       
        static async Task Main(string[] args)
        {
            var ad = new AlterDiceClient(new AlterDiceClientOptions("", "") 
            { 
                LogWriters=new System.Collections.Generic.List<System.IO.TextWriter>() { new  ThreadSafeFileWriter("log.txt"), new DebugTextWriter()  },
                LogVerbosity=LogVerbosity.Debug
            
            });
            var ob = new AlterDiceSymbolOrderBook("BTCUSDT", ad, new AlterDiceOrderBookOptions("42", 3051)
            {
                LogWriters = new System.Collections.Generic.List<System.IO.TextWriter>() { new ThreadSafeFileWriter("orderbook.txt"), new DebugTextWriter() },
                LogVerbosity = LogVerbosity.Debug

            });
            ob.OnOrderBookUpdate += Ob_OnOrderBookUpdate;
            ob.Start();           

            Console.ReadLine();
        }

        private static void Ob_OnOrderBookUpdate((System.Collections.Generic.IEnumerable<CryptoExchange.Net.Interfaces.ISymbolOrderBookEntry> Bids, System.Collections.Generic.IEnumerable<CryptoExchange.Net.Interfaces.ISymbolOrderBookEntry> Asks) obj)
        {
            Console.WriteLine(DateTime.UtcNow);
        }
    }
}
