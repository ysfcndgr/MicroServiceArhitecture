using System;
using Shared.Events.Common;

namespace Shared.Events
{
	public class StockReserverdEvent : IEvent
	{
		public Guid BuyerId { get; set; }
		public Guid OrderId { get; set; }
		public decimal TotalPrice { get; set; }
	}
}

