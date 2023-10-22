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
	public interface IUserRepository : IBaseRepository<User>
	{

	}

	public class UserRepository : BaseRepository<User>, IUserRepository
	{
		public UserRepository(DbContextOptions<DataContext> options) : base(options)
		{
		}
	}
}
