using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
	public class OrderRepository : IOrderRepository
	{
		private readonly DataContext dataContext;

		public OrderRepository(DataContext dataContext) 
		{ 
			this.dataContext = dataContext;
		}

		public async Task<List<Order>> GetAllAsync()
		{
			return await dataContext.Orders.Where(n => !n.Deleted).ToListAsync();
		}

		public async Task<bool> AddAsync(Order model)
		{
			if (model != null)
			{
				dataContext.Orders.Add(model);

				var result = await dataContext.SaveChangesAsync();

				if(result > 0)
					return true;
			} 

			return false;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var model = dataContext.Orders.FirstOrDefault(n => n.Id == id);

			if (model != null && !model.Deleted)
			{
				model.Deleted = true;

				dataContext.Orders.Update(model);

				var result = await dataContext.SaveChangesAsync();

				if (result > 0)
					return true;
			}

			return false;
		}

		public async Task<bool> UpdateAsync(Order model)
		{
			if (model != null && !model.Deleted)
			{
				dataContext.Orders.Update(model);

				var result = await dataContext.SaveChangesAsync();

				if (result > 0)
					return true;
			}

			return false;
		}

		public async Task<Order?> GetByIdAsync(Guid id)
		{
			return await Task.FromResult(dataContext.Orders.FirstOrDefault(n => n.Id == id));
		}
	}
}
