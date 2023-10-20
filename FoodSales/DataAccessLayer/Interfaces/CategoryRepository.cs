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
	public interface ICategoryRepository : IBaseRepository<Category>
	{

	}

	public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(DbContextOptions<DataContext> options) : base(options)
		{
		}
	}
}
