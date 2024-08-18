using System;
using Microsoft.EntityFrameworkCore;
using Order.API.Models.Entities;

namespace Order.API.Models
{
	public class OrderAPIDbContext : DbContext
	{
        public OrderAPIDbContext(DbContextOptions<OrderAPIDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }
	}
}

