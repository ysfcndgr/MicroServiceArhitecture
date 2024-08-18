using System;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly OrderAPIDbContext _orderAPIDBContext;

        public PaymentFailedEventConsumer(OrderAPIDbContext orderAPIDBContext)
        {
            _orderAPIDBContext = orderAPIDBContext;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            {
                Models.Entities.Order order = await _orderAPIDBContext.Orders.FirstOrDefaultAsync(x => x.OrderId == context.Message.OrderId);
                order.OrderStatus = Models.Enums.OrderStatus.Failed;
                await _orderAPIDBContext.SaveChangesAsync();
            }
        }
    }
}
