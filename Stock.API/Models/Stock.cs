using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Stock.API.Models
{
	public class Stock
	{
		[BsonId]
		[BsonGuidRepresentation(MongoDB.Bson.GuidRepresentation.CSharpLegacy)]
		[BsonElement(Order = 0)]
		public Guid Id { get; set; }

        [BsonGuidRepresentation(MongoDB.Bson.GuidRepresentation.CSharpLegacy)]
        [BsonElement(Order = 1)]
        public Guid ProductId { get; set; }

		[BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        [BsonElement(Order = 2)]
        public int Count { get; set; }
	}
}

