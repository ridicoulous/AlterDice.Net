using AlterDice.Net;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
                LogWriters = new System.Collections.Generic.List<System.IO.TextWriter>() { new ThreadSafeFileWriter("log.txt"), new DebugTextWriter() },
                LogVerbosity = LogVerbosity.Debug
            });
            var ob = new AlterDiceSymbolOrderBook("BTCUSDT", ad, new AlterDiceOrderBookOptions("42", 3051)
            {
                LogWriters = new System.Collections.Generic.List<System.IO.TextWriter>() { new ThreadSafeFileWriter("orderbook.txt"), new DebugTextWriter() },
                LogVerbosity = LogVerbosity.Debug

            });
            ob.OnOrderBookUpdate += Ob_OnOrderBookUpdate;
            ob.Start();
            Console.Read();
           // var hist = await ad.GetOrdersHistoryAsync();

           
            Console.ReadLine();
        }
        private static List<ISymbolOrderBookEntry> _lastAsks = new List<ISymbolOrderBookEntry>();
        private static List<ISymbolOrderBookEntry> _lastBids = new List<ISymbolOrderBookEntry>();

        private static void Ob_OnOrderBookUpdate((System.Collections.Generic.IEnumerable<CryptoExchange.Net.Interfaces.ISymbolOrderBookEntry> Bids, System.Collections.Generic.IEnumerable<CryptoExchange.Net.Interfaces.ISymbolOrderBookEntry> Asks) obj)
        {
            if (!_lastAsks.Any() || !_lastBids.Any())
            {
                _lastBids.Clear();
                _lastAsks.Clear();
                foreach (var a in obj.Asks)
                    _lastAsks.Add(a);

                foreach (var a in obj.Bids)
                    _lastBids.Add(a);
            }

            Console.WriteLine($"{DateTime.UtcNow}: {obj.Asks.Except(_lastAsks, new ObEqu()).Count()} new asks detected");
            Console.WriteLine($"{DateTime.UtcNow}: {obj.Bids.Except(_lastBids, new ObEqu()).Count()} new bids detected");

            _lastBids.Clear();
            _lastAsks.Clear();
            foreach (var a in obj.Asks)
                _lastAsks.Add(a);

            foreach (var a in obj.Bids)
                _lastBids.Add(a);

        }
        public class ObEqu : IEqualityComparer<ISymbolOrderBookEntry>
        {
            public bool Equals([AllowNull] ISymbolOrderBookEntry x, [AllowNull] ISymbolOrderBookEntry y)
            {
                return x.Price == y.Price && y.Quantity == x.Quantity;
            }

            public int GetHashCode([DisallowNull] ISymbolOrderBookEntry obj)
            {
                return obj.Price.GetHashCode() ^ obj.Quantity.GetHashCode();
            }
        }
    }
}
