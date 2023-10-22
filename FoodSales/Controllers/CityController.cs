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
	public class CityController : Controller
	{
		private readonly IRegionService _regionService;
		private readonly ICityService _cityService;
		public CityController(IRegionService _regionService, ICityService _cityService)
		{
			this._regionService = _regionService;
			this._cityService = _cityService;
		}

		[HttpGet(nameof(GetCities))]
		public async Task<ApiResult<List<SelectListModel>>> GetCities(Guid? regionId)
		{
			return await _cityService.GetCitiesAsync(regionId);
		}

		[HttpPost(nameof(CityAdd))]
		public async Task<ApiResult<string>> CityAdd(CityDto model)
		{
			return await _cityService.AddAsync(model);
		}

		[HttpPost(nameof(CityUpdate))]
		public async Task<ApiResult<string>> CityUpdate(CityUpdate model)
		{
			return await _cityService.UpdateAsync(model);
		}

		[HttpDelete("{id:Guid}")]
		public async Task<ApiResult<string>> CityDelete(Guid id)
		{
			return await _cityService.DeleteAsync(id);
		}
	}
}
