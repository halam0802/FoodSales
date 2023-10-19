using BusinessLogicLayer.Model;
using BusinessLogicLayer.Services;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FoodSales.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly IRegionService regionService;
		public ProductController(IRegionService regionService)
		{
			this.regionService = regionService;
		}

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

		[HttpDelete("{id:Guid}")]
		public async Task<bool> RegionDelete(Guid id)
		{
			return await regionService.DeleteAsync(id);
		}
		#endregion
	}
}
