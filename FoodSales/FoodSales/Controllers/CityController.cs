using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodSales.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class CityController : Controller
	{
		private readonly IRegionService regionService;
		private readonly ICityService cityService;
		public CityController(IRegionService regionService, ICityService cityService)
		{
			this.regionService = regionService;
			this.cityService = cityService;
		}

		#region Region

		[HttpGet(nameof(GetRegions))]
		public async Task<ApiResult<List<SelectListModel>>> GetRegions()
		{
			return await regionService.GetRegionsAsync();
		}

		[HttpPost(nameof(RegionAdd))]
		public async Task<ApiResult<string>> RegionAdd(RegionDto model)
		{
			return await regionService.AddAsync(model);
		}

		[HttpPost(nameof(RegionUpdate))]
		public async Task<ApiResult<string>> RegionUpdate(RegionUpdate model)
		{
			return await regionService.UpdateAsync(model);
		}

		[HttpDelete("{regionid:Guid}")]
		public async Task<ApiResult<string>> RegionDelete(Guid regionid)
		{
			return await regionService.DeleteAsync(regionid);
		}
		#endregion

		#region City

		[HttpGet(nameof(GetCities))]
		public async Task<ApiResult<List<SelectListModel>>> GetCities(Guid regionId)
		{
			return await cityService.GetCitiesAsync(regionId);
		}

		[HttpPost(nameof(CityAdd))]
		public async Task<ApiResult<string>> CityAdd(CityDto model)
		{
			return await cityService.AddAsync(model);
		}

		[HttpPost(nameof(CityUpdate))]
		public async Task<ApiResult<string>> CityUpdate(CityUpdate model)
		{
			return await cityService.UpdateAsync(model);
		}

		[HttpDelete("{cityid:Guid}")]
		public async Task<ApiResult<string>> CityDelete(Guid cityid)
		{
			return await cityService.DeleteAsync(cityid);
		}
		#endregion
	}
}
