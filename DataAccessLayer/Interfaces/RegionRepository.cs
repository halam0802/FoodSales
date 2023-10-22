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
	public interface IRegionRepository : IBaseRepository<Region>
	{

	}

	public class RegionRepository : BaseRepository<Region>, IRegionRepository
	{
		public RegionRepository(DbContextOptions<DataContext> options) : base(options)
		{
		}
	}
}
