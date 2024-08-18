using System;
using MongoDB.Driver;

namespace Stock.API.Services
{
	public class MongoDBService
	{
		private readonly IMongoDatabase _mongoDatabase;

		public MongoDBService(IConfiguration configuration)
		{
			MongoClient mongoClient = new(configuration.GetConnectionString("MongoDB"));
			_mongoDatabase = mongoClient.GetDatabase("StockAPIDB");
		}

		public IMongoCollection<T> GetCollection<T>() => _mongoDatabase.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
		
	}
}

