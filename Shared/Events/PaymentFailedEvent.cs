using System;
using Shared.Events.Common;

namespace Shared.Events
{
	public class PaymentFailedEvent : IEvent
	{
		public Guid OrderId { get; set; }
		public string Message { get; set; }
	}
}

