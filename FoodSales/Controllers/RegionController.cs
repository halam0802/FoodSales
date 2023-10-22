using BusinessLogicLayer.Models.Dto;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace FoodSales.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class RegionController : Controller
	{
		private readonly IRegionService _regionService;
		public RegionController(IRegionService _regionService)
		{
			this._regionService = _regionService;
		}

		[HttpGet(nameof(GetRegions))]
		public async Task<ApiResult<List<SelectListModel>>> GetRegions()
		{
			return await _regionService.GetRegionsAsync();
		}

		[HttpPost(nameof(RegionAdd))]
		public async Task<ApiResult<string>> RegionAdd(RegionDto model)
		{
			return await _regionService.AddAsync(model);
		}

		[HttpPost(nameof(RegionUpdate))]
		public async Task<ApiResult<string>> RegionUpdate(RegionUpdate model)
		{
			return await _regionService.UpdateAsync(model);
		}

		[HttpDelete("{id:Guid}")]
		public async Task<ApiResult<string>> RegionDelete(Guid id)
		{
			return await _regionService.DeleteAsync(id);
		}

	}
}
