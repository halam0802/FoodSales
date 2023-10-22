using BusinessLogicLayer.Common;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Dto;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System.Data;
using System.Linq.Dynamic.Core;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLogicLayer
{
    public class FoodSaleService: IFoodSaleService
	{
		private readonly IFoodSaleRepository _foodSaleRepository;
		protected readonly ILogger _logger;

		private readonly ICityService _cityService;
		private readonly ICategoryService _categoryService;
		private readonly IProductService _productService;
		private readonly IRegionService _regionService;
		public FoodSaleService(IFoodSaleRepository _foodSaleRepository, ILoggerFactory loggerFactory, ICityService _cityService, 
			ICategoryService _categoryService, IProductService _productService, IRegionService _regionService) 
		{ 
			this._foodSaleRepository = _foodSaleRepository;
			_logger = loggerFactory.CreateLogger<FoodSaleService>();

			this._cityService = _cityService;
			this._categoryService = _categoryService;
			this._productService = _productService;
			this._regionService = _regionService;

		}

		/// <summary>
		/// FoodSale pagination list
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public async Task<ApiResult<PagingModel<FoodSaleItem>>> FoodSalePaging(FoodSaleFilter request)
		{
			try
			{
				var result = new PagingModel<FoodSaleItem>
				{
					Data = new List<FoodSaleItem>(),
					PageIndex = request.PageIndex,
					PageSize = request.PageSize,
					TotalPages = 0,
				};

				#region Query
				var dataSources = _foodSaleRepository.Table.Where(n => !n.Deleted);

				if (!string.IsNullOrEmpty(request.ProductName))
					dataSources = dataSources.Where(n => !string.IsNullOrEmpty(n.ProductName) && n.ProductName.Contains(request.ProductName));

				if (!string.IsNullOrEmpty(request.CategoryName))
					dataSources = dataSources.Where(n => !string.IsNullOrEmpty(n.CategoryName) && n.CategoryName.Contains(request.CategoryName));

				if (!string.IsNullOrEmpty(request.CityName))
					dataSources = dataSources.Where(n => !string.IsNullOrEmpty(n.CityName) && n.CityName.Contains(request.CityName));

				if (!string.IsNullOrEmpty(request.RegionName))
					dataSources = dataSources.Where(n => !string.IsNullOrEmpty(n.RegionName) && n.RegionName.Contains(request.RegionName));

				if (request.Quantity > 0)
					dataSources = dataSources.Where(n => n.Quantity == request.Quantity);

				if (request.UnitPrice > 0)
					dataSources = dataSources.Where(n => n.UnitPrice == request.UnitPrice);

				if (request.TotalPrice > 0)
					dataSources = dataSources.Where(n => n.TotalPrice == request.TotalPrice);

				if (!string.IsNullOrEmpty(request.FromDate))
				{
					var fDateUtc = Convert.ToDateTime(request.FromDate).ToUniversalTime();

					dataSources = dataSources.Where(n => n.CreatedAt >= fDateUtc);
				}

				if (!string.IsNullOrEmpty(request.ToDate))
				{
					var tDateUtc = Convert.ToDateTime(request.ToDate);

					if (request.ToDate == request.FromDate)
						tDateUtc = tDateUtc.AddDays(1);

					dataSources = dataSources.Where(n => n.CreatedAt <= tDateUtc.ToUniversalTime());
				}
				#endregion

				#region Sort
				dataSources = dataSources.OrderBy($"CreatedAt {request.SortDate},RegionName {request.SortRegion},CityName {request.SortCity},CategoryName {request.SortCategory},ProductName {request.SortProduct},Quantity {request.SortQuantity},UnitPrice {request.SortUnitPrice},TotalPrice {request.SortTotalPrice}");
				#endregion

				if (dataSources == null || !dataSources.Any())
					return ApiResult<PagingModel<FoodSaleItem>>.Successfully(result);

				//pagination
				var datas = await PagedList<FoodSale>.Create(dataSources, request.PageIndex, request.PageSize);

				//results return
				result.TotalPages = datas.TotalPages;
				result.PageIndex = datas.PageIndex;
				result.PageSize = datas.PageSize;
				result.Data = datas.Select(n => new FoodSaleItem
				{
					Id = n.Id,
					CreateAt = n.CreatedAt,
					CategoryName = n.CategoryName,
					CityName = n.CityName,
					ProductName = n.ProductName,
					RegionName = n.RegionName,
					Quantity = n.Quantity,
					UnitPrice = n.UnitPrice,
					TotalPrice = n.TotalPrice,
				}).ToList();

				return ApiResult<PagingModel<FoodSaleItem>>.Successfully(result);
			}
			catch (Exception ex)
			{
				_logger.LogError($"FoodSalePaging ERROR: {ex.Message}", ex);

				return ApiResult<PagingModel<FoodSaleItem>>.ErrorInProcessing(ex.Message);
			}
		}

		#region Import file excel
		/// <summary>
		/// Import file excel
		/// </summary>
		/// <param name="importFile"></param>
		/// <returns><see cref="string"/></returns>
		public async Task<ApiResult<string>> ImportFileExcel(IFormFile importFile)
		{
			if (importFile == null)
				return ApiResult<string>.Failure("File not found!");

			MemoryStream memoryStream = new MemoryStream();

			try
			{
				//check the file extension
				var extension = Path.GetExtension(importFile.FileName)?.ToLower();

				//return error if not xlsx
				if (string.IsNullOrEmpty(extension) || !extension.Equals(".xlsx") || extension.Length == 0)
					return ApiResult<string>.Failure("File format is incorrect!");

				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

				await importFile.OpenReadStream().CopyToAsync(memoryStream);

				using (var reader = ExcelReaderFactory.CreateReader(memoryStream, new ExcelReaderConfiguration { FallbackEncoding = Encoding.UTF8 }))
				{
					var totalRows = reader.RowCount;
					var totalColumns = reader.FieldCount;

					if (totalRows < 2 || totalColumns != 8)
						return ApiResult<string>.Failure("File format is incorrect!");

					var result = reader.AsDataSet();
					if (result == null)
						return ApiResult<string>.Failure("File format is incorrect!");

					return await ReadTableCollection(result.Tables, totalRows);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"ImportFileExcel ERROR: {ex.Message}", ex);

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
			finally
			{
				if (memoryStream != null)
				{
					memoryStream.Flush();
					memoryStream.Close();
					memoryStream.Dispose();
				}
			}
		}

		/// <summary>
		/// Read table collection
		/// </summary>
		/// <param name="item"></param>
		/// <param name="totalRows"></param>
		/// <returns></returns>
		private async Task<ApiResult<string>> ReadTableCollection(DataTableCollection tableCollection, int totalRows)
		{
			try
			{
				if (tableCollection == null || totalRows == 0)
					return ApiResult<string>.Failure();

				var dataTable = tableCollection[0];

				var allRegion = await _regionService.GetAllAsync();
				var allCity = await _cityService.GetAllAsync();
				var allCategory = await _categoryService.GetAllAsync();
				var allProduct = await _productService.GetAllAsync();

				var foodSaleList = new List<FoodSale>();

				for (var i = 1; i < totalRows; i++)
				{
					var row = dataTable.Rows[i];

					#region Validate data row

					//Get data of each column
					var date = row[0].ToString();
					var regionName = row[1].ToString();
					var cityName = row[2].ToString();
					var categoryName = row[3].ToString();
					var productName = row[4].ToString();
					var quantity = row[5].ToString();
					var unitPrice = row[6].ToString();
					var totalPrice = row[7].ToString();

					if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(regionName) || string.IsNullOrEmpty(cityName) || string.IsNullOrEmpty(categoryName) || string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(quantity) || string.IsNullOrEmpty(unitPrice) || string.IsNullOrEmpty(totalPrice))
						continue;

					if (!DateTime.TryParse(date, out DateTime orderDate)) continue;

					if (!int.TryParse(quantity, out int intQuantity) || intQuantity <= 0) continue;

					if (!decimal.TryParse(unitPrice, out decimal uPrice) || uPrice <= 0) continue;

					if (!decimal.TryParse(totalPrice, out decimal tPrice) || tPrice <= 0) continue;

					//Check the name in the database
					var objRegion = allRegion.FirstOrDefault(n => n.Name == regionName.Trim());
					if (objRegion == null) continue;

					var objCity = allCity.FirstOrDefault(n => n.Name == cityName.Trim());
					if (objCity == null) continue;

					var objCategory = allCategory.FirstOrDefault(n => n.Name == categoryName.Trim());
					if (objCategory == null) continue;

					var objProduct = allProduct.FirstOrDefault(n => n.Name == productName.Trim());
					if (objProduct == null) continue;
					#endregion

					#region Add new obj to list
					foodSaleList.Add(new FoodSale
					{
						Id = Guid.NewGuid(),
						RegionId = objRegion.Id,
						CityId = objCity.Id,
						CategoryId = objCategory.Id,
						ProductId = objProduct.Id,
						RegionName = objRegion.Name,
						CityName = objCity.Name,
						CategoryName = objCategory.Name,
						ProductName = objProduct.Name,
						Quantity = intQuantity,
						UnitPrice = Math.Round(uPrice,2),
						TotalPrice = Math.Round(tPrice, 2),
						CreatedAt = orderDate.ToUniversalTime()
					});
					#endregion
					
				}

				//Add list object into db
				if (foodSaleList.Count > 0)
				{
					var result = await _foodSaleRepository.AddRangeAsync(foodSaleList);

					if (result.isSuccess)
						return ApiResult<string>.Successfully($"Successfully added {foodSaleList.Count} records");
				}

				return ApiResult<string>.Failure();
			}
			catch (Exception ex)
			{
				return ApiResult<string>.Failure(ex.Message);
			}	
		}
		#endregion

		#region Get one
		/// <summary>
		/// Get object by guid id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<FoodSale?> GetByIdAsync(Guid id)
		{
			try
			{
				var obj = await _foodSaleRepository.GetByIdAsync(id);

				return obj;
			}
			catch (Exception ex)
			{
				_logger.LogError($"FoodSale GetByIdAsync ERROR: {ex.Message}");

				return null;
			}
		}

		/// <summary>
		/// Get FoodSale detail by guid id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<FoodSaleDetail?> GetDetailAsync(Guid id)
		{
			try
			{
				var obj = await GetByIdAsync(id);

				if(obj != null)
				{
					return new FoodSaleDetail
					{
						Id = obj.Id,
						CategoryId = obj.CategoryId,
						CityId = obj.CityId,
						ProductId = obj.ProductId,
						RegionId = obj.RegionId,
						Quantity = obj.Quantity,
						TotalPrice = obj.TotalPrice,
						UnitPrice = obj.UnitPrice,
					};
				}
			}
			catch (Exception ex)
			{
				_logger.LogError($"FoodSale GetDetailAsync ERROR: {ex.Message}");
			}

			return null;
		}
		#endregion

		#region CRUD

		/// <summary>
		/// Add new FoodSale
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<ApiResult<string>> AddAsync(FoodSaleDto model)
		{
			try
			{
				#region Validate
				var objRegion = await _regionService.GetByIdAsync(model.RegionId);
				if(objRegion == null)
					return ApiResult<string>.Failure("Region do not exist!");

				var objCity = await _cityService.GetByIdAsync(model.CityId);
				if (objCity == null)
					return ApiResult<string>.Failure("City do not exist!");

				if(objCity.RegionId != objRegion.Id)
					return ApiResult<string>.Failure($"City {objCity.Name} does not belong to region {objRegion.Name}!");

				var objCategory = await _categoryService.GetByIdAsync(model.CategoryId);
				if (objCategory == null)
					return ApiResult<string>.Failure("Category do not exist!");

				var objProduct = await _productService.GetByIdAsync(model.ProductId);
				if (objProduct == null)
					return ApiResult<string>.Failure("Region do not exist!");

				if (objProduct.CategoryId != objCategory.Id)
					return ApiResult<string>.Failure($"Product {objProduct.Name} does not belong to category {objCategory.Name}!");

				if(model.Quantity <= 0)
					return ApiResult<string>.Failure("Quantity must be > 0!");

				if (model.UnitPrice <= 0)
					return ApiResult<string>.Failure("UnitPrice must be > 0!");
				#endregion

				var newObj = new FoodSale
				{
					Id = Guid.NewGuid(),
					Deleted = false,
					RegionId = objRegion.Id,
					RegionName = objRegion.Name,
					CityId = objCity.Id,
					CityName = objCity.Name,
					CategoryId = objCategory.Id,
					CategoryName = objCategory.Name,
					ProductId = objProduct.Id,
					ProductName = objProduct.Name,
					Quantity = model.Quantity,
					UnitPrice = model.UnitPrice,
					TotalPrice = model.Quantity * model.UnitPrice				
				};

				var result = await _foodSaleRepository.AddAsync(newObj);

				if (!result.isSuccess)
					return ApiResult<string>.Failure(result.Message);

				return ApiResult<string>.Successfully(newObj.Id.ToString());
			}
			catch (Exception ex)
			{
				_logger.LogError($"FoodSale AddAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}

		/// <summary>
		/// update FoodSale
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<ApiResult<string>> UpdateAsync(FoodSaleUpdate model)
		{
			try
			{
				#region Validate
				var objRegion = await _regionService.GetByIdAsync(model.RegionId);
				if (objRegion == null)
					return ApiResult<string>.Failure("Region do not exist!");

				var objCity = await _cityService.GetByIdAsync(model.CityId);
				if (objCity == null)
					return ApiResult<string>.Failure("City do not exist!");

				if (objCity.RegionId != objRegion.Id)
					return ApiResult<string>.Failure($"City {objCity.Name} does not belong to region {objRegion.Name}!");

				var objCategory = await _categoryService.GetByIdAsync(model.CategoryId);
				if (objCategory == null)
					return ApiResult<string>.Failure("Category do not exist!");

				var objProduct = await _productService.GetByIdAsync(model.ProductId);
				if (objProduct == null)
					return ApiResult<string>.Failure("Region do not exist!");

				if (objProduct.CategoryId != objCategory.Id)
					return ApiResult<string>.Failure($"Product {objProduct.Name} does not belong to category {objCategory.Name}!");

				if (model.Quantity <= 0)
					return ApiResult<string>.Failure("Quantity must be > 0!");

				if (model.UnitPrice <= 0)
					return ApiResult<string>.Failure("UnitPrice must be > 0!");

				var objFoodSale = await GetByIdAsync(model.Id);
				if (objFoodSale == null)
					return ApiResult<string>.Failure("Object do not exist!");
				#endregion

				objFoodSale.RegionId = objRegion.Id;
				objFoodSale.RegionName = objRegion.Name;
				objFoodSale.CityId = objCity.Id;
				objFoodSale.CityName = objCity.Name;
				objFoodSale.CategoryId = objCategory.Id;
				objFoodSale.CategoryName = objCategory.Name;
				objFoodSale.ProductId = objProduct.Id;
				objFoodSale.ProductName = objProduct.Name;
				objFoodSale.Quantity = model.Quantity;
				objFoodSale.UnitPrice = model.UnitPrice;
				objFoodSale.TotalPrice = model.Quantity * model.UnitPrice;
				objFoodSale.UpdatedAt = DateTime.UtcNow;

				var result = await _foodSaleRepository.UpdateAsync(objFoodSale);

				if (!result.isSuccess)
					return ApiResult<string>.Failure(result.Message);

				return ApiResult<string>.Successfully(objFoodSale.Id.ToString());
			}
			catch (Exception ex)
			{
				_logger.LogError($"FoodSale UpdateAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}

		/// <summary>
		/// Delete FoodSale
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

					var result = await _foodSaleRepository.UpdateAsync(obj);

					if (!result.isSuccess)
						return ApiResult<string>.Failure(result.Message);

					return ApiResult<string>.Successfully(obj.Id.ToString());
				}

				return ApiResult<string>.Failure();
			}
			catch (Exception ex)
			{
				_logger.LogError($"FoodSale DeleteAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}

		/// <summary>
		/// Delete Many FoodSale
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<ApiResult<string>> DeleteManyAsync(Guid[]? ids)
		{
			try
			{
				if(ids != null && ids.Any())
				{
					var dataSources = _foodSaleRepository.Table.Where(n => !n.Deleted && ids.Contains(n.Id));

					if (dataSources != null && dataSources.Any())
					{
						dataSources.ToList().ToList().ForEach(c =>
						{
							c.Deleted = true;
						});

						var result = await _foodSaleRepository.SaveChangeAsync();

						if(result > 0)
							return ApiResult<string>.Successfully("OK");
					}
				}

				return ApiResult<string>.Failure();
			}
			catch (Exception ex)
			{
				_logger.LogError($"FoodSale DeleteManyAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}
		#endregion
	}
}