using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodSales.Controllers
{
    [Authorize]
	[ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly ICategoryService categoryService;
		private readonly IProductService productService;
		private readonly IFoodSaleService orderService;
		public ProductController(ICategoryService categoryService,IProductService productService, IFoodSaleService orderService)
		{
			this.categoryService = categoryService;
			this.productService = productService;
			this.orderService = orderService;
		}

		#region Order

		//[HttpGet(nameof(GetOrders))]
		//public async Task<List<SelectListModel>> GetOrders()
		//{
		//	return await orderService.GetOrdersAsync();
		//}

		//[HttpPost(nameof(OrderAdd))]
		//public async Task<bool> OrderAdd(OrderDto model)
		//{
		//	return await orderService.AddAsync(model);
		//}

		//[HttpPost(nameof(OrderUpdate))]
		//public async Task<bool> OrderUpdate(OrderUpdate model)
		//{
		//	return await orderService.UpdateAsync(model);
		//}

		//[HttpDelete("{orderid:Guid}")]
		//public async Task<bool> OrderDelete(Guid orderid)
		//{
		//	return await orderService.DeleteAsync(orderid);
		//}
		#endregion

		#region Product

		[HttpGet(nameof(GetProducts))]
		public async Task<ApiResult<List<SelectListModel>>> GetProducts(Guid categoryId)
		{
			return await productService.GetProductsAsync(categoryId);
		}

		[HttpPost(nameof(ProductAdd))]
		public async Task<ApiResult<string>> ProductAdd(ProductDto model)
		{
			return await productService.AddAsync(model);
		}

		[HttpPost(nameof(ProductUpdate))]
		public async Task<ApiResult<string>> ProductUpdate(ProductUpdate model)
		{
			return await productService.UpdateAsync(model);
		}

		[HttpDelete("{productid:Guid}")]
		public async Task<ApiResult<string>> ProductDelete(Guid productid)
		{
			return await productService.DeleteAsync(productid);
		}
		#endregion

		#region Category

		[HttpGet(nameof(GetCategories))]
		public async Task<ApiResult<List<SelectListModel>>> GetCategories()
		{
			return await categoryService.GetCategoriesAsync();
		}

		[HttpPost(nameof(CategoryAdd))]
		public async Task<ApiResult<string>> CategoryAdd(RegionDto model)
		{
			return await categoryService.AddAsync(model);
		}

		[HttpPost(nameof(CategoryUpdate))]
		public async Task<ApiResult<string>> CategoryUpdate(RegionUpdate model)
		{
			return await categoryService.UpdateAsync(model);
		}

		[HttpDelete("{categoryid:Guid}")]
		public async Task<ApiResult<string>> CategoryDelete(Guid categoryid)
		{
			return await categoryService.DeleteAsync(categoryid);
		}
		#endregion
	}
}
