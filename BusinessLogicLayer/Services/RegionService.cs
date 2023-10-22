using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Dto;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer
{
    public class RegionService : IRegionService
	{
		private readonly IRegionRepository _regionRepository;
		protected readonly ILogger _logger;

		public RegionService(IRegionRepository _regionRepository, ILoggerFactory loggerFactory)
		{
			this._regionRepository = _regionRepository;
			_logger = loggerFactory.CreateLogger<RegionService>();
		}

		/// <summary>
		/// Get region list
		/// </summary>
		/// <returns></returns>
		public async Task<ApiResult<List<SelectListModel>>> GetRegionsAsync()
		{
			try
			{
				var list = _regionRepository.Table.Where(n => !n.Deleted);

				var result = list.Select(n => new SelectListModel
				{
					ItemText = n.Name,
					ItemValue = n.Id.ToString()
				}).ToList();

				return ApiResult<List<SelectListModel>>.Successfully(result);
			}
			catch (Exception ex)
			{
				_logger.LogError($"GetRegionsAsync ERROR: {ex.Message}");

				return await Task.FromResult(ApiResult<List<SelectListModel>>.ErrorInProcessing(ex.Message));
			}
		}

		/// <summary>
		/// Get all
		/// </summary>
		/// <returns></returns>
		public async Task<List<Region>> GetAllAsync()
		{
			try
			{
				var list = _regionRepository.Table.Where(n => !n.Deleted);

				return list.ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError($"GetAllAsync ERROR: {ex.Message}");

				return await Task.FromResult(new List<Region>());
			}
		}

		/// <summary>
		/// Get object by name
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<Region?> GetByNameAsync(string name)
		{
			try
			{
				var obj = _regionRepository.Table.FirstOrDefault(n => !n.Deleted && n.Name == name);

				return await Task.FromResult(obj);
			}
			catch (Exception ex)
			{
				_logger.LogError($"GetRegionByNameAsync ERROR: {ex.Message}");

				return null;
			}
		}

		/// <summary>
		/// Get object by guid id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<Region?> GetByIdAsync(Guid id)
		{
			try
			{
				var obj = await _regionRepository.GetByIdAsync(id);

				return obj;
			}
			catch (Exception ex)
			{
				_logger.LogError($"GetRegionByIdAsync ERROR: {ex.Message}");

				return null;
			}
		}

		/// <summary>
		/// Add new region
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<ApiResult<string>> AddAsync(RegionDto model)
		{
			try
			{
				var objByName = await GetByNameAsync(model.Name);
				if (objByName != null)
					return ApiResult<string>.Failure("Name already exists!");

				var newObj = new Region
				{
					Id = Guid.NewGuid(),
					Name = model.Name,
				};

				var result = await _regionRepository.AddAsync(newObj);

				if (!result.isSuccess)
					return ApiResult<string>.Failure(result.Message);

				return ApiResult<string>.Successfully(newObj.Name);
			}
			catch (Exception ex)
			{
				_logger.LogError($"RegionAddAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}

		/// <summary>
		/// update region
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
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

						var result = await _regionRepository.UpdateAsync(obj);

						if (!result.isSuccess)
							return ApiResult<string>.Failure(result.Message);

						return ApiResult<string>.Successfully(obj.Name);
					}
				}

				return ApiResult<string>.Failure();
			}
			catch (Exception ex)
			{
				_logger.LogError($"RegionUpdateAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}

		/// <summary>
		/// Delete region
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<ApiResult<string>> DeleteAsync(Guid id)
		{
			try
			{
				var obj = await GetByIdAsync(id);
				if (obj != null)
				{
					obj.Deleted = true;
					obj.UpdatedAt = DateTime.UtcNow;

					var result = await _regionRepository.UpdateAsync(obj);

					if (!result.isSuccess)
						return ApiResult<string>.Failure(result.Message);

					return ApiResult<string>.Successfully(obj.Name);
				}

				return ApiResult<string>.Failure();
			}
			catch (Exception ex)
			{
				_logger.LogError($"RegionDeleteAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}
	}
}