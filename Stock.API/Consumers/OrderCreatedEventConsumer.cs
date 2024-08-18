using System;
using MassTransit;
using MongoDB.Driver;
using Shared.Constants;
using Shared.Events;
using Shared.Messages;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        IMongoCollection<Models.Stock> _stockCollection;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(IMongoCollection<Models.Stock> stockCollection, ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint)
        {
            _stockCollection = stockCollection;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();

            foreach (OrderItemMessage orderItem in context.Message.OrderItems)
            {
              stockResult.Add((await _stockCollection
              .FindAsync(s => s.ProductId == orderItem.ProductId
                && s.Count >= orderItem.Count))
                .Any());
            }

            if (stockResult.TrueForAll(sr => sr.Equals(true)))
            {
                foreach (OrderItemMessage orderItem in context.Message.OrderItems)
                {
                 Models.Stock stock = await (await _stockCollection
                  .FindAsync(s => s.ProductId == orderItem.ProductId))
                  .FirstOrDefaultAsync();
                    stock.Count -= orderItem.Count;
                    await _stockCollection
                    .FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId, stock);
                }

                StockReserverdEvent stockReserverdEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    TotalPrice = context.Message.TotalPrice
                };

              ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue}"));
              await sendEndpoint.Send(stockReserverdEvent);
            }

            else
            {
                StockNotReservedEvent stockNotReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    Message = "Hata oluştu"
                };
                await _publishEndpoint.Publish(stockNotReservedEvent);

            }

           await Task.CompletedTask;
        }
    }
}

