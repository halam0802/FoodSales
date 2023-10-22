using BusinessLogicLayer.Common;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Dto;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FoodSales.Controllers
{
    [Authorize]
	[ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly IProductService _productService;
		public ProductController(IProductService _productService)
		{
			this._productService = _productService;
		}

		[HttpGet(nameof(GetProducts))]
		public async Task<ApiResult<List<SelectListModel>>> GetProducts(Guid? categoryId)
		{
			return await _productService.GetProductsAsync(categoryId);
		}

		[HttpPost(nameof(ProductAdd))]
		public async Task<ApiResult<string>> ProductAdd(ProductDto model)
		{
			return await _productService.AddAsync(model);
		}

		[HttpPost(nameof(ProductUpdate))]
		public async Task<ApiResult<string>> ProductUpdate(ProductUpdate model)
		{
			return await _productService.UpdateAsync(model);
		}

		[HttpDelete("{id:Guid}")]
		public async Task<ApiResult<string>> ProductDelete(Guid id)
		{
			return await _productService.DeleteAsync(id);
		}	
	}
}
