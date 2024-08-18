using System;
namespace Shared.Messages
{
	public class OrderItemMessage
	{
		public Guid ProductId { get; set; }
		public int Count { get; set; }
	}
}

