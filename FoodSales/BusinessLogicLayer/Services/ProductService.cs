﻿using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Models.ViewModels;
using Microsoft.Extensions.Logging;

namespace BusinessLogicLayer.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
		protected readonly ILogger _logger;

		public ProductService(IProductRepository _productRepository, ILoggerFactory loggerFactory)
		{
			this._productRepository = _productRepository;
			_logger = loggerFactory.CreateLogger<ProductService>();
		}
		public async Task<ApiResult<List<SelectListModel>>> GetProductsAsync(Guid categoryId)
		{
			try
			{
				var list = _productRepository.Table.Where(n => !n.Deleted && n.CategoryId == categoryId);


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
		public async Task<Product?> GetByNameAsync(string name)
		{
			try
			{
				var obj = _productRepository.Table.FirstOrDefault(n => !n.Deleted && n.Name == name);

				return await Task.FromResult(obj);
			}
			catch (Exception ex)
			{
				_logger.LogError($"GetProductByNameAsync ERROR: {ex.Message}");

				return null;
			}
		}
		public async Task<Product?> GetByIdAsync(Guid id)
		{
			try
			{
				var obj = await _productRepository.GetByIdAsync(id);

				return obj;
			}
			catch (Exception ex)
			{
				_logger.LogError($"GetProductByIdAsync ERROR: {ex.Message}");

				return null;
			}
		}

		public async Task<ApiResult<string>> AddAsync(ProductDto model)
		{
			try
			{
				var objByName = await GetByNameAsync(model.Name);
				if (objByName != null)
					return ApiResult<string>.Failure("Name already exists!");

				var newObj = new Product
				{
					Id = Guid.NewGuid(),
					Name = model.Name,
				};

				var result = await _productRepository.AddAsync(newObj);

				if (!result.isSuccess)
					return ApiResult<string>.Failure(result.Message);

				return ApiResult<string>.Successfully(newObj.Name);
			}
			catch (Exception ex)
			{
				_logger.LogError($"ProductAddAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}

		public async Task<ApiResult<string>> UpdateAsync(ProductUpdate model)
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

						var result = await _productRepository.UpdateAsync(obj);

						if (!result.isSuccess)
							return ApiResult<string>.Failure(result.Message);

						return ApiResult<string>.Successfully(obj.Name);
					}
				}

				return ApiResult<string>.Failure();
			}
			catch (Exception ex)
			{
				_logger.LogError($"ProductUpdateAsync ERROR: {ex.Message}");

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
					var result = await _productRepository.DeleteAsync(obj);

					if (!result.isSuccess)
						return ApiResult<string>.Failure(result.Message);

					return ApiResult<string>.Successfully(obj.Name);
				}

				return ApiResult<string>.Failure();
			}
			catch (Exception ex)
			{
				_logger.LogError($"ProductDeleteAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}
	}
}