using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
	public class CityRepository : ICityRepository
	{
		private readonly DataContext dataContext;

		public CityRepository(DataContext dataContext) 
		{ 
			this.dataContext = dataContext;
		}

		public async Task<List<City>> GetAllAsync()
		{
			return await dataContext.Cities.Where(n => !n.Deleted).ToListAsync();
		}

		public async Task<bool> AddAsync(City model)
		{
			if (model != null)
			{
				dataContext.Cities.Add(model);

				var result = await dataContext.SaveChangesAsync();

				if(result > 0)
					return true;
			} 

			return false;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var model = dataContext.Cities.FirstOrDefault(n => n.Id == id);

			if (model != null && !model.Deleted)
			{
				model.Deleted = true;

				dataContext.Cities.Update(model);

				var result = await dataContext.SaveChangesAsync();

				if (result > 0)
					return true;
			}

			return false;
		}

		public async Task<bool> UpdateAsync(City model)
		{
			if (model != null && !model.Deleted)
			{
				dataContext.Cities.Update(model);

				var result = await dataContext.SaveChangesAsync();

				if (result > 0)
					return true;
			}

			return false;
		}

		public async Task<City?> GetByIdAsync(Guid id)
		{
			return await Task.FromResult(dataContext.Cities.FirstOrDefault(n => n.Id == id));
		}
	}
}
