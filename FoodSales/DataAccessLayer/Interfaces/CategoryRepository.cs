using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly DataContext dataContext;

		public CategoryRepository(DataContext dataContext) 
		{ 
			this.dataContext = dataContext;
		}


		public async Task<List<Category>> GetAllAsync()
		{
			return await dataContext.Categories.Where(n => !n.Deleted).ToListAsync();
		}

		public async Task<bool> AddAsync(Category model)
		{
			if (model != null)
			{
				dataContext.Categories.Add(model);

				var result = await dataContext.SaveChangesAsync();

				if(result > 0)
					return true;
			} 

			return false;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var model = dataContext.Categories.FirstOrDefault(n => n.Id == id);

			if (model != null && !model.Deleted)
			{
				model.Deleted = true;

				dataContext.Categories.Update(model);

				var result = await dataContext.SaveChangesAsync();

				if (result > 0)
					return true;
			}

			return false;
		}

		public async Task<bool> UpdateAsync(Category model)
		{
			if (model != null && !model.Deleted)
			{
				dataContext.Categories.Update(model);

				var result = await dataContext.SaveChangesAsync();

				if (result > 0)
					return true;
			}

			return false;
		}

		public async Task<Category?> GetByIdAsync(Guid id)
		{
			return await Task.FromResult(dataContext.Categories.FirstOrDefault(n => n.Id == id));
		}
	}
}
