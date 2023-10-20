using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace DataAccessLayer
{
	public class DataContext : DbContext
	{
		public DataContext()
		{
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

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//var connection = DataSettingsManager.LoadSettings();
			//var connectionString = connection != null && !string.IsNullOrEmpty(connection.ConnectionString) ? connection.ConnectionString : "";

			if (!optionsBuilder.IsConfigured)
				optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS; Initial Catalog=FoodSales;Trusted_Connection=True;TrustServerCertificate=true");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}
	}
}