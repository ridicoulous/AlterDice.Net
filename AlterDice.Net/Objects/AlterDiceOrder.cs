﻿using AlterDice.Net.Converters;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.ExchangeInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlterDice.Net.Objects
{
    public class AlterDiceOrder : ICommonOrder
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("type")]
        public AlterDiceOrderSide OrderSide { get; set; }

        [JsonProperty("status")]
        public AlterDiceOrderStatus Status { get; set; }

        [JsonProperty("type_trade")]
        public AlterDiceOrderType OrderType { get; set; }

        [JsonProperty("pair_name")]
        public string Symbol { get; set; }

        [JsonProperty("volume"), JsonConverter(typeof(Decimal10e8Converter))]
        public virtual decimal Quantity { get; set; }

        [JsonProperty("volume_done"), JsonConverter(typeof(Decimal10e8Converter))]
        public virtual decimal QuantityDone { get; set; }

        [JsonProperty("price"), JsonConverter(typeof(Decimal10e8Converter))]
        public virtual decimal QuoteQuantity { get; set; }
        [JsonProperty("price_done"), JsonConverter(typeof(Decimal10e8Converter))]
        public virtual decimal? QuoteQuantityFilled { get; set; }

        [JsonProperty("rate"), JsonConverter(typeof(Decimal10e8Converter))]
        public virtual decimal Price { get; set; }

        [JsonProperty("time_create"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("time_done"), JsonConverter(typeof(TimestampSecondsConverter))]
        public DateTime? ExecutedAt { get; set; }

        public string CommonId => Id.ToString();

        public string CommonSymbol => Symbol;

        public decimal CommonPrice => Price;

        public decimal CommonQuantity => Quantity;

        public bool IsActive => Status == AlterDiceOrderStatus.Active || Status == AlterDiceOrderStatus.InProcess;

        public IExchangeClient.OrderSide CommonSide => OrderSide == AlterDiceOrderSide.Buy ? IExchangeClient.OrderSide.Buy : IExchangeClient.OrderSide.Sell;

        public IExchangeClient.OrderType CommonType => OrderType switch
        {
            AlterDiceOrderType.Limit => IExchangeClient.OrderType.Limit,
            AlterDiceOrderType.Market => IExchangeClient.OrderType.Market,
            _ => IExchangeClient.OrderType.Other
        };

        public DateTime CommonOrderTime => CreatedAt;

        public IExchangeClient.OrderStatus CommonStatus => Status switch
        {
            AlterDiceOrderStatus.Active => IExchangeClient.OrderStatus.Active,
            AlterDiceOrderStatus.InProcess => IExchangeClient.OrderStatus.Active,
            AlterDiceOrderStatus.Canceled => IExchangeClient.OrderStatus.Canceled,
            AlterDiceOrderStatus.Filled => IExchangeClient.OrderStatus.Filled,
            _ => throw new NotImplementedException("Undefined order status")
        };
    }
}
