using System;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {

        private readonly OrderAPIDbContext _orderAPIDBContext;

        public PaymentCompletedEventConsumer(OrderAPIDbContext orderAPIDBContext)
        {
            _orderAPIDBContext = orderAPIDBContext;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            Models.Entities.Order order = await _orderAPIDBContext.Orders.FirstOrDefaultAsync(x => x.OrderId == context.Message.OrderId);
            order.OrderStatus = Models.Enums.OrderStatus.Completed;
            await _orderAPIDBContext.SaveChangesAsync();
        }
    }
}

