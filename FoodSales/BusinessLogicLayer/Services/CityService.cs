using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Models.ViewModels;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer
{
	public class CityService : ICityService
	{
		private readonly ICityRepository _cityRepository;
		protected readonly ILogger _logger;

		public CityService(ICityRepository _cityRepository, ILoggerFactory loggerFactory)
		{
			this._cityRepository = _cityRepository;
			_logger = loggerFactory.CreateLogger<CityService>();
		}

		public async Task<ApiResult<List<SelectListModel>>> GetCitiesAsync(Guid regionId)
		{
			try
			{
				var dataList = _cityRepository.Table.Where(n => !n.Deleted && n.RegionId == regionId);

				var result = dataList != null ? dataList.Select(n => new SelectListModel
				{
					ItemText = n.Name,
					ItemValue = n.Id.ToString()
				}).ToList() : new List<SelectListModel>();

				return ApiResult<List<SelectListModel>>.Successfully(result);
			}
			catch (Exception ex)
			{
				_logger.LogError($"GetCitiesAsync ERROR: {ex.Message}");

				return await Task.FromResult(ApiResult<List<SelectListModel>>.ErrorInProcessing(ex.Message));
			}
		}
		public async Task<City?> GetByNameAsync(string name)
		{
			try
			{
				var obj = _cityRepository.Table.FirstOrDefault(n => !n.Deleted && n.Name == name);

				return await Task.FromResult(obj);
			}
			catch (Exception ex)
			{
				_logger.LogError($"GetCityByNameAsync ERROR: {ex.Message}");

				return null;
			}
		}
		public async Task<City?> GetByIdAsync(Guid id)
		{
			try
			{
				var obj = await _cityRepository.GetByIdAsync(id);

				return obj;
			}
			catch (Exception ex)
			{
				_logger.LogError($"GetCityByIdAsync ERROR: {ex.Message}");

				return null;
			}
		}

		public async Task<ApiResult<string>> AddAsync(CityDto model)
		{
			try
			{
				var objByName = await GetByNameAsync(model.Name);
				if (objByName != null)
					return ApiResult<string>.Failure("Name already exists!");

				var newObj = new City
				{
					Id = Guid.NewGuid(),
					Name = model.Name,
				};

				var result = await _cityRepository.AddAsync(newObj);

				if (!result.isSuccess)
					return ApiResult<string>.Failure(result.Message);

				return ApiResult<string>.Successfully(newObj.Name);
			}
			catch (Exception ex)
			{
				_logger.LogError($"CityAddAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}

		public async Task<ApiResult<string>> UpdateAsync(CityUpdate model)
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

						var result = await _cityRepository.UpdateAsync(obj);

						if (!result.isSuccess)
							return ApiResult<string>.Failure(result.Message);

						return ApiResult<string>.Successfully(obj.Name);
					}
				}

				return ApiResult<string>.Failure();
			}
			catch (Exception ex)
			{
				_logger.LogError($"CityUpdateAsync ERROR: {ex.Message}");

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
					var result = await _cityRepository.DeleteAsync(obj);

					if (!result.isSuccess)
						return ApiResult<string>.Failure(result.Message);

					return ApiResult<string>.Successfully(obj.Name);
				}

				return ApiResult<string>.Failure();
			}
			catch (Exception ex)
			{
				_logger.LogError($"CityDeleteAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}
	}
}