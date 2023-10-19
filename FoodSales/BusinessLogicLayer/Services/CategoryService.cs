using BusinessLogicLayer.Model;
using BusinessLogicLayer.Services;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer
{
	public class CategoryService: ICategoryService
	{
		private readonly ICategoryRepository CategoryRepository;

		public CategoryService(ICategoryRepository CategoryRepository) 
		{ 
			this.CategoryRepository = CategoryRepository;
		}

		public async Task<List<SelectListModel>> GetCategorysAsync()
		{
			var list = await CategoryRepository.GetAllAsync();

			return list.Select(n => new SelectListModel
			{
				ItemText = n.Name,
				ItemValue = n.Id.ToString()
			}).ToList();
		}

		public async Task<Category?> GetByIdAsync(Guid id)
		{
			return await CategoryRepository.GetByIdAsync(id);
		}

		public async Task<bool> AddAsync(RegionDto model)
		{
			var newObj = new Category
			{
				Id = Guid.NewGuid(),
				Name = model.Name,
			};
			
			return await CategoryRepository.AddAsync(newObj);
		}
	
		public async Task<bool> UpdateAsync(RegionUpdate model)
		{
			if (model.Id != null)
			{
				var obj = await GetByIdAsync(model.Id.Value);

				if (obj != null)
				{
					obj.Name = model.Name ?? string.Empty;
					obj.UpdatedAt = DateTime.UtcNow;

					return await CategoryRepository.UpdateAsync(obj);
				}
			}
			
			return false;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			return await CategoryRepository.DeleteAsync(id);
		}
	}
}