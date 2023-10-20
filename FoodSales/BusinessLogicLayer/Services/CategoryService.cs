using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer
{
    public class CategoryService: ICategoryService
	{
		private readonly ICategoryRepository _categoryRepository;
		protected readonly ILogger _logger;

		public CategoryService(ICategoryRepository _categoryRepository, ILoggerFactory loggerFactory) 
		{ 
			this._categoryRepository = _categoryRepository;
			_logger = loggerFactory.CreateLogger<CategoryService>();
		}

		public async Task<ApiResult<List<SelectListModel>>> GetCategoriesAsync()
		{
			try
			{
				var list = _categoryRepository.Table.Where(n => !n.Deleted);

				var result = list.Select(n => new SelectListModel
				{
					ItemText = n.Name,
					ItemValue = n.Id.ToString()
				}).ToList();

				return ApiResult<List<SelectListModel>>.Successfully(result);
			}
			catch (Exception ex)
			{
				_logger.LogError($"GetCategoriesAsync ERROR: {ex.Message}");

				return await Task.FromResult(ApiResult<List<SelectListModel>>.ErrorInProcessing(ex.Message));
			}
		}

		public async Task<Category?> GetByIdAsync(Guid id)
		{
			try
			{
				var obj = await _categoryRepository.GetByIdAsync(id);

				return obj;
			}
			catch (Exception ex)
			{
				_logger.LogError($"GetCategoryByIdAsync ERROR: {ex.Message}");

				return null;
			}
		}

		public async Task<Category?> GetByNameAsync(string name)
		{
			try
			{
				var obj = _categoryRepository.Table.FirstOrDefault(n => !n.Deleted && n.Name == name);

				return await Task.FromResult(obj);
			}
			catch (Exception ex)
			{
				_logger.LogError($"GetCategoryByNameAsync ERROR: {ex.Message}");

				return null;
			}
		}

		public async Task<ApiResult<string>> AddAsync(RegionDto model)
		{
			try
			{
				var objByName = await GetByNameAsync(model.Name);
				if(objByName != null)
					return ApiResult<string>.Failure("Name already exists!");

				var newObj = new Category
				{
					Id = Guid.NewGuid(),
					Name = model.Name,
				};

				var result = await _categoryRepository.AddAsync(newObj);

				if(!result.isSuccess)
					return ApiResult<string>.Failure(result.Message);

				return ApiResult<string>.Successfully(newObj.Name);
			}
			catch (Exception ex)
			{
				_logger.LogError($"CategoryAddAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}
	
		public async Task<ApiResult<string>> UpdateAsync(RegionUpdate model)
		{
			try
			{
				var objByName = await GetByNameAsync(model.Name);
				if (objByName != null && objByName.Id != model.Id)
					return ApiResult<string>.Failure("Name already exists!");

				if (model.Id != null)
				{
					var obj = await GetByIdAsync(model.Id.Value);

					if (obj != null)
					{
						obj.Name = model.Name ?? string.Empty;
						obj.UpdatedAt = DateTime.UtcNow;

						var result = await _categoryRepository.UpdateAsync(obj);

						if (!result.isSuccess)
							return ApiResult<string>.Failure(result.Message);

						return ApiResult<string>.Successfully(obj.Name);
					}
				}
		
				return ApiResult<string>.Failure();
			}
			catch (Exception ex)
			{
				_logger.LogError($"CategoryUpdateAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}

		public async Task<ApiResult<string>> DeleteAsync(Guid id)
		{
			try
			{
				var obj = await GetByIdAsync(id);
				if (obj != null)
				{
					var result = await _categoryRepository.DeleteAsync(obj);

					if (!result.isSuccess)
						return ApiResult<string>.Failure(result.Message);

					return ApiResult<string>.Successfully(obj.Name);
				}

				return ApiResult<string>.Failure();
			}
			catch (Exception ex)
			{
				_logger.LogError($"CategoryDeleteAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}
	}
}