using BusinessLogicLayer.Common;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodSales.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class FoodSaleController : Controller
	{
		private readonly IFoodSaleService _foodSaleService;
		public FoodSaleController( IFoodSaleService _foodSaleService)
		{
			this._foodSaleService = _foodSaleService;
		}

		[HttpPost(nameof(FoodSalePaging))]
		public async Task<ApiResult<PagingModel<FoodSaleItem>>> FoodSalePaging(FoodSaleFilter model)
		{
			return await _foodSaleService.FoodSalePaging(model);
		}

		[HttpPost(nameof(ImportFileExcel))]
		public async Task<ApiResult<string>> ImportFileExcel(IFormFile importFile)
		{
			return await _foodSaleService.ImportFileExcel(importFile);
		}

		[HttpGet(nameof(FoodSaleDetail))]
		public async Task<FoodSaleDetail?> FoodSaleDetail(Guid id)
		{
			return await _foodSaleService.GetDetailAsync(id);
		}

		[HttpPost(nameof(FoodSaleAdd))]
		public async Task<ApiResult<string>> FoodSaleAdd(FoodSaleDto model)
		{
			return await _foodSaleService.AddAsync(model);
		}

		[HttpPost(nameof(FoodSaleUpdate))]
		public async Task<ApiResult<string>> FoodSaleUpdate(FoodSaleUpdate model)
		{
			return await _foodSaleService.UpdateAsync(model);
		}

		[HttpDelete("{id:Guid}")]
		public async Task<ApiResult<string>> FoodSaleDelete(Guid id)
		{
			return await _foodSaleService.DeleteAsync(id);
		}

		[HttpPost(nameof(FoodSaleDeleteMany))]
		public async Task<ApiResult<string>> FoodSaleDeleteMany(FoodSaleDeleteMany model)
		{
			return await _foodSaleService.DeleteManyAsync(model.Ids);
		}


	}
}
