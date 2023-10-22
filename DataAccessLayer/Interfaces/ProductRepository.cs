using DataAccessLayer.Base;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
	public interface IProductRepository : IBaseRepository<Product>
	{

	}

	public class ProductRepository : BaseRepository<Product>, IProductRepository
	{
		public ProductRepository(DbContextOptions<DataContext> options) : base(options)
		{
		}
	}
}
