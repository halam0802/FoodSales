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
	public interface IFoodSaleRepository : IBaseRepository<FoodSale>
	{

	}

	public class FoodSaleRepository : BaseRepository<FoodSale>, IFoodSaleRepository
	{
		public FoodSaleRepository(DbContextOptions<DataContext> options) : base(options)
		{
		}
	}
}
