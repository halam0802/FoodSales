using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
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
		private readonly IRegionService regionService;
		private readonly ICategoryService categoryService;
		private readonly ICityService cityService;
		private readonly IProductService productService;
		private readonly IOrderService orderService;
		public ProductController(IRegionService regionService, ICategoryService categoryService, ICityService cityService, IProductService productService, IOrderService orderService)
		{
			this.regionService = regionService;
			this.categoryService = categoryService;
			this.cityService = cityService;
			this.productService = productService;
			this.orderService = orderService;
		}

		#region Order

		[HttpGet(nameof(GetOrders))]
		public async Task<List<SelectListModel>> GetOrders()
		{
			return await orderService.GetOrdersAsync();
		}

		[HttpPost(nameof(OrderAdd))]
		public async Task<bool> OrderAdd(OrderDto model)
		{
			return await orderService.AddAsync(model);
		}

		[HttpPost(nameof(OrderUpdate))]
		public async Task<bool> OrderUpdate(OrderUpdate model)
		{
			return await orderService.UpdateAsync(model);
		}

		[HttpDelete("{orderid:Guid}")]
		public async Task<bool> OrderDelete(Guid orderid)
		{
			return await orderService.DeleteAsync(orderid);
		}
		#endregion

		#region Product

		[HttpGet(nameof(GetProducts))]
		public async Task<List<SelectListModel>> GetProducts()
		{
			return await productService.GetProductsAsync();
		}

		[HttpPost(nameof(ProductAdd))]
		public async Task<bool> ProductAdd(ProductDto model)
		{
			return await productService.AddAsync(model);
		}

		[HttpPost(nameof(ProductUpdate))]
		public async Task<bool> ProductUpdate(ProductUpdate model)
		{
			return await productService.UpdateAsync(model);
		}

		[HttpDelete("{productid:Guid}")]
		public async Task<bool> ProductDelete(Guid productid)
		{
			return await productService.DeleteAsync(productid);
		}
		#endregion

		#region Region

		[HttpGet(nameof(GetRegions))]
		public async Task<List<SelectListModel>> GetRegions()
		{
			return await regionService.GetRegionsAsync();
		}

		[HttpPost(nameof(RegionAdd))]
		public async Task<bool> RegionAdd(RegionDto model)
		{
			return await regionService.AddAsync(model);
		}

		[HttpPost(nameof(RegionUpdate))]
		public async Task<bool> RegionUpdate(RegionUpdate model)
		{
			return await regionService.UpdateAsync(model);
		}

		[HttpDelete("{regionid:Guid}")]
		public async Task<bool> RegionDelete(Guid regionid)
		{
			return await regionService.DeleteAsync(regionid);
		}
		#endregion

		#region City

		[HttpGet(nameof(GetCities))]
		public async Task<List<SelectListModel>> GetCities()
		{
			return await cityService.GetCitiesAsync();
		}

		[HttpPost(nameof(CityAdd))]
		public async Task<bool> CityAdd(CityDto model)
		{
			return await cityService.AddAsync(model);
		}

		[HttpPost(nameof(CityUpdate))]
		public async Task<bool> CityUpdate(CityUpdate model)
		{
			return await cityService.UpdateAsync(model);
		}

		[HttpDelete("{cityid:Guid}")]
		public async Task<bool> CityDelete(Guid cityid)
		{
			return await cityService.DeleteAsync(cityid);
		}
		#endregion

		#region Category

		[HttpGet(nameof(GetCategories))]
		public async Task<List<SelectListModel>> GetCategories()
		{
			return await categoryService.GetCategoriesAsync();
		}

		[HttpPost(nameof(CategoryAdd))]
		public async Task<bool> CategoryAdd(RegionDto model)
		{
			return await categoryService.AddAsync(model);
		}

		[HttpPost(nameof(CategoryUpdate))]
		public async Task<bool> CategoryUpdate(RegionUpdate model)
		{
			return await categoryService.UpdateAsync(model);
		}

		[HttpDelete("{categoryid:Guid}")]
		public async Task<bool> CategoryDelete(Guid categoryid)
		{
			return await categoryService.DeleteAsync(categoryid);
		}
		#endregion
	}
}
