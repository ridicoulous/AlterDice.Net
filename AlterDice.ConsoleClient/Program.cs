using AlterDice.Net;
using AlterDice.Net.Objects;
using CryptoExchange.Net.ExchangeInterfaces;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using Newtonsoft.Json;
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
            var book = new AlterDiceSymbolOrderBook("BTCUSDT", new AlterDiceOrderBookOptions("BTCUSDT", 3051));

            //    book.OnOrderBookUpdate += Book_OnOrderBookUpdate;
            book.OnBestOffersChanged += Book_OnBestOffersChanged;
            await book.StartAsync();
            Console.ReadLine();
            book.Dispose();
        }

        private static void Book_OnBestOffersChanged((ISymbolOrderBookEntry BestBid, ISymbolOrderBookEntry BestAsk) obj)
        {
            Console.WriteLine($"{obj.BestBid.Price}:{obj.BestAsk.Price} - [{obj.BestBid.Quantity}:{obj.BestAsk.Quantity}]");
        }

        private static void Book_OnOrderBookUpdate((IEnumerable<ISymbolOrderBookEntry> Bids, IEnumerable<ISymbolOrderBookEntry> Asks) obj)
        {
            //Console.WriteLine(JsonConvert.SerializeObject(obj));
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
