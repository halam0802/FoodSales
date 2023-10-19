using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
	public class RegionRepository : IRegionRepository
	{
		private readonly DataContext dataContext;

		public RegionRepository(DataContext dataContext) 
		{ 
			this.dataContext = dataContext;
		}

		public async Task<List<Region>> GetAllAsync()
		{
			return await dataContext.Regions.Where(n => !n.Deleted).ToListAsync();
		}

		public async Task<bool> AddAsync(Region model)
		{
			if (model != null)
			{
				dataContext.Regions.Add(model);

				var result = await dataContext.SaveChangesAsync();

				if(result > 0)
					return true;
			} 

			return false;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var model = dataContext.Regions.FirstOrDefault(n => n.Id == id);

			if (model != null && !model.Deleted)
			{
				model.Deleted = true;

				dataContext.Regions.Update(model);

				var result = await dataContext.SaveChangesAsync();

				if (result > 0)
					return true;
			}

			return false;
		}

		public async Task<bool> UpdateAsync(Region model)
		{
			if (model != null && !model.Deleted)
			{
				dataContext.Regions.Update(model);

				var result = await dataContext.SaveChangesAsync();

				if (result > 0)
					return true;
			}

			return false;
		}

		public async Task<Region?> GetByIdAsync(Guid id)
		{
			return await Task.FromResult(dataContext.Regions.FirstOrDefault(n => n.Id == id));
		}
	}
}
