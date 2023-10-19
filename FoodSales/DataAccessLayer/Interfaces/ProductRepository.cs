using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
	public class ProductRepository : IProductRepository
	{
		private readonly DataContext dataContext;

		public ProductRepository(DataContext dataContext) 
		{ 
			this.dataContext = dataContext;
		}

		public async Task<List<Product>> GetAllAsync()
		{
			return await dataContext.Products.Where(n => !n.Deleted).ToListAsync();
		}

		public async Task<bool> AddAsync(Product model)
		{
			if (model != null)
			{
				dataContext.Products.Add(model);

				var result = await dataContext.SaveChangesAsync();

				if(result > 0)
					return true;
			} 

			return false;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var model = dataContext.Products.FirstOrDefault(n => n.Id == id);

			if (model != null && !model.Deleted)
			{
				model.Deleted = true;

				dataContext.Products.Update(model);

				var result = await dataContext.SaveChangesAsync();

				if (result > 0)
					return true;
			}

			return false;
		}

		public async Task<bool> UpdateAsync(Product model)
		{
			if (model != null && !model.Deleted)
			{
				dataContext.Products.Update(model);

				var result = await dataContext.SaveChangesAsync();

				if (result > 0)
					return true;
			}

			return false;
		}

		public async Task<Product?> GetByIdAsync(Guid id)
		{
			return await Task.FromResult(dataContext.Products.FirstOrDefault(n => n.Id == id));
		}
	}
}
