using System;
using MassTransit;
using Shared.Events;

namespace Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReserverdEvent>
    {
        readonly IPublishEndpoint _publishEndpoint;

        public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<StockReserverdEvent> context)
        {
            //Ödeme işlemleri

            if (true)
            {
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderId = context.Message.OrderId
                };
               await _publishEndpoint.Publish(paymentCompletedEvent);
            }
            else
            {
                PaymentFailedEvent paymentFailedEvent = new()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Ödeme yok"
                };
                await _publishEndpoint.Publish(paymentFailedEvent);
            }


            await Task.CompletedTask;
        }
    }
}

