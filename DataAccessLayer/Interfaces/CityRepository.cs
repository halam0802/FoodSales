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
	public interface ICityRepository : IBaseRepository<City>
	{

	}

	public class CityRepository : BaseRepository<City>, ICityRepository
	{
		public CityRepository(DbContextOptions<DataContext> options) : base(options)
		{
		}
	}
}
