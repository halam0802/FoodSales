using BusinessLogicLayer.Models.Dto;
using BusinessLogicLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BusinessLogicLayer.Interfaces;

namespace FoodSales.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]")]
	public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;
		public CategoryController(ICategoryService _categoryService)
		{
			this._categoryService = _categoryService;
		}

		[HttpGet(nameof(GetCategories))]
		public async Task<ApiResult<List<SelectListModel>>> GetCategories()
		{
			return await _categoryService.GetCategoriesAsync();
		}

		[HttpPost(nameof(CategoryAdd))]
		public async Task<ApiResult<string>> CategoryAdd(RegionDto model)
		{
			return await _categoryService.AddAsync(model);
		}

		[HttpPost(nameof(CategoryUpdate))]
		public async Task<ApiResult<string>> CategoryUpdate(RegionUpdate model)
		{
			return await _categoryService.UpdateAsync(model);
		}

		[HttpDelete("{id:Guid}")]
		public async Task<ApiResult<string>> CategoryDelete(Guid id)
		{
			return await _categoryService.DeleteAsync(id);
		}

	}
}
