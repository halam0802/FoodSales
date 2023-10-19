using BusinessLogicLayer.Model;
using BusinessLogicLayer.Services;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer
{
	public class OrderService: IOrderService
	{
		private readonly IOrderRepository OrderRepository;

		public OrderService(IOrderRepository OrderRepository) 
		{ 
			this.OrderRepository = OrderRepository;
		}

		public async Task<Order?> GetByIdAsync(Guid id)
		{
			return await OrderRepository.GetByIdAsync(id);
		}

		public async Task<bool> AddAsync(OrderDto model)
		{
			var newObj = new Order
			{
				Id = Guid.NewGuid()				
			};
			
			return await OrderRepository.AddAsync(newObj);
		}
	
		public async Task<bool> UpdateAsync(OrderUpdate model)
		{
			if (model.Id != null)
			{
				var obj = await GetByIdAsync(model.Id.Value);

				if (obj != null)
				{
					//obj.Name = model.Name ?? string.Empty;
					obj.UpdatedAt = DateTime.UtcNow;

					return await OrderRepository.UpdateAsync(obj);
				}
			}
			
			return false;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			return await OrderRepository.DeleteAsync(id);
		}

		public Task<List<SelectListModel>> GetOrdersAsync()
		{
			throw new NotImplementedException();
		}
	}
}