using System;
using Order.API.Models.Enums;

namespace Order.API.Models.Entities
{
	public class Order
	{
		public Guid OrderId { get; set; }
		public Guid BuyerId { get; set; }
		public decimal TotalPrice { get; set; }
		public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
		public OrderStatus OrderStatus { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}

