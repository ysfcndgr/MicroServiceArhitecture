using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Order.API.Models;
using Order.API.Models.Entities;
using Order.API.ViewModels;
using Shared.Events;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly OrderAPIDbContext _context;
        private IPublishEndpoint _publishEndPoint;

        public OrdersController(OrderAPIDbContext context, IPublishEndpoint publishEndPoint)
        {
            _context = context;
            _publishEndPoint = publishEndPoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderVM createOrderVM)
        {
            Models.Entities.Order order = new()
            {
                OrderId = Guid.NewGuid(),
                BuyerId = createOrderVM.BuyerId,
                CreatedDate = DateTime.Now,
                OrderStatus = Models.Enums.OrderStatus.Suspend
            };

            order.OrderItems = createOrderVM.OrderItems.Select(oi => new OrderItem {
                Count = oi.Count,
                Price = oi.Price,
                ProductId = oi.ProductId
            }).ToList();

            order.TotalPrice = createOrderVM.OrderItems.Sum(oi => (oi.Price * oi.Count));

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            OrderCreatedEvent orderCreatedEvent = new()
            {
                BuyerId = createOrderVM.BuyerId,
                OrderId = order.OrderId,
                OrderItems = order.OrderItems.Select(oi => new Shared.Messages.OrderItemMessage
                {
                    Count = oi.Count,
                    ProductId = oi.ProductId
                }).ToList(),
                TotalPrice = order.TotalPrice
            };

            await _publishEndPoint.Publish(orderCreatedEvent);

            return Ok();
        }

    }
}

