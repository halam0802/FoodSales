using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace DataAccessLayer
{
	public class DataContext : DbContext
	{
		public string connectionString = string.Empty;
		public DataContext(string conn)
		{
			this.connectionString = conn;
		}
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Region> Regions { get; set; }
		public DbSet<City> Cities { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<FoodSale> FoodSales { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//"Data Source=.\\SQLEXPRESS; Initial Catalog=FoodSales;Trusted_Connection=True;TrustServerCertificate=true"
			if (!optionsBuilder.IsConfigured)
				optionsBuilder.UseSqlServer(connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}
	}
}