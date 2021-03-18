using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDiceBalancesResponse : AlterDiceBaseResponse<AlterDiceCurrenciesResult>
    {

    }
    public class AlterDiceCurrenciesResult
    {
        [JsonProperty("list")]
        public List<AlterDiceBalance> Result { get; set; }
    }
    public class AlterDiceBalance : ICommonBalance
    {
        private decimal _balance;
        private decimal _balanceAvailable;

        [JsonProperty("balance")]
        public decimal Balance 
        { 
            get
            {
                return _balance;
            }
            set 
            {
                _balance = value / 1e8m;
            }
        }

        [JsonProperty("balance_available")]
        public decimal BalanceAvailable
        {
            get
            {
                return _balanceAvailable;
            }
            set
            {
                _balanceAvailable = value / 1e8m;
            }
        }

        [JsonProperty("currency")]
        public AlterDiceCurrency Currency { get; set; }

        public string CommonAsset => Currency.ShortName;

        public decimal CommonAvailable => BalanceAvailable;

        public decimal CommonTotal => Balance;
    }
    public class AlterDiceCurrency
    {
        [JsonProperty("iso3")]
        public string ShortName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
