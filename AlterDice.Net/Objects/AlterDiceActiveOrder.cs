using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AlterDice.Net.Objects
{
    public class AlterDiceActiveOrder : ICommonOrder
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type")]
        public AlterDiceOrderSide OrderSide { get; set; }

        [JsonProperty("status")]
        public AlterDiceOrderStatus Status { get; set; }

        [JsonProperty("type_trade")]
        public AlterDiceOrderType OrderType { get; set; }

        [JsonProperty("pair")]
        public string Symbol { get; set; }

        [JsonProperty("volume")]
        public virtual decimal Quantity { get; set; }

        [JsonProperty("volume_done")]
        public virtual decimal QuantityDone { get; set; }

        [JsonProperty("price")]
        public virtual decimal QuoteQuantity { get; set; }
        [JsonProperty("price_done")]
        public virtual decimal? QuoteQuantityFilled { get; set; }

        [JsonProperty("rate")]
        public virtual decimal Price { get; set; }

        [JsonProperty("time_create"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("time_done"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime? ExecutedAt { get; set; }


        public string CommonId => Id.ToString();

        public string CommonSymbol => Symbol;

        public decimal CommonPrice => Price;

        public decimal CommonQuantity => Quantity;

        public IExchangeClient.OrderStatus CommonStatus => Status switch
        {
            AlterDiceOrderStatus.Active => IExchangeClient.OrderStatus.Active,
            AlterDiceOrderStatus.InProcess => IExchangeClient.OrderStatus.Active,
            AlterDiceOrderStatus.Canceled => IExchangeClient.OrderStatus.Canceled,
            AlterDiceOrderStatus.Filled => IExchangeClient.OrderStatus.Filled,
            _ => throw new NotImplementedException("Undefined order status")
        };

        public bool IsActive => Status == 0;

        public IExchangeClient.OrderSide CommonSide => OrderSide == AlterDiceOrderSide.Buy ? IExchangeClient.OrderSide.Buy : IExchangeClient.OrderSide.Sell;

        public IExchangeClient.OrderType CommonType => OrderType switch
        {
            AlterDiceOrderType.Limit => IExchangeClient.OrderType.Limit,
            AlterDiceOrderType.Market => IExchangeClient.OrderType.Market,
            _ => IExchangeClient.OrderType.Other
        };

        [JsonProperty("list_trades")]
        public List<AlterDiceOrderTrade> Trades { get; set; }

        public DateTime CommonOrderTime => CreatedAt;
    }
}