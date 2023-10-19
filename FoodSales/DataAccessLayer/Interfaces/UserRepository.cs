using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
	public class UserRepository : IUserRepository
	{
		private readonly DataContext dataContext;

		public UserRepository(DataContext dataContext) 
		{ 
			this.dataContext = dataContext;
		}

		public async Task<List<User>> GetAllAsync()
		{
			return await dataContext.Users.Where(n => !n.Deleted).ToListAsync();
		}

		public async Task<bool> AddAsync(User model)
		{
			if (model != null)
			{
				dataContext.Users.Add(model);

				var result = await dataContext.SaveChangesAsync();

				if(result > 0)
					return true;
			} 

			return false;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var model = dataContext.Users.FirstOrDefault(n => n.Id == id);

			if (model != null && !model.Deleted)
			{
				model.Deleted = true;

				dataContext.Users.Update(model);

				var result = await dataContext.SaveChangesAsync();

				if (result > 0)
					return true;
			}

			return false;
		}

		public async Task<bool> UpdateAsync(User model)
		{
			if (model != null && !model.Deleted)
			{
				dataContext.Users.Update(model);

				var result = await dataContext.SaveChangesAsync();

				if (result > 0)
					return true;
			}

			return false;
		}

		public async Task<User?> GetByIdAsync(Guid id)
		{
			return await Task.FromResult(dataContext.Users.FirstOrDefault(n => n.Id == id));
		}
	}
}
