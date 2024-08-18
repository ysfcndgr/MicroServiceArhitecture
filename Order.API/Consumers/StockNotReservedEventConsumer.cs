using System;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {

        private readonly OrderAPIDbContext _orderAPIDBContext;

        public StockNotReservedEventConsumer(OrderAPIDbContext orderAPIDBContext)
        {
            _orderAPIDBContext = orderAPIDBContext;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            Models.Entities.Order order = await _orderAPIDBContext.Orders.FirstOrDefaultAsync(x => x.OrderId == context.Message.OrderId);
            order.OrderStatus = Models.Enums.OrderStatus.Failed;
            await _orderAPIDBContext.SaveChangesAsync();
        }
    }
}

