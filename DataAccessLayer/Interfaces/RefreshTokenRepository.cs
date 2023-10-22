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
	public interface IRefreshTokenRepository : IBaseRepository<RefreshToken>
	{

	}

	public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
	{
		public RefreshTokenRepository(DbContextOptions<DataContext> options) : base(options)
		{
		}
	}
}
