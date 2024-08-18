using System;
namespace Order.API.ViewModels
{
	public record CreateOrderVM
	{
		public Guid BuyerId { get; set; }
		public List<CreateOrderItemVM> OrderItems { get; set; } = new List<CreateOrderItemVM>();
	}

	public record CreateOrderItemVM
	{
		public Guid ProductId { get; set; }
		public int Count { get; set; }
		public decimal Price { get; set; }
	}

}

