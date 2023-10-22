using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Dto;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICategoryService
    {
		/// <summary>
		/// Get category list
		/// </summary>
		/// <returns></returns>
		Task<ApiResult<List<SelectListModel>>> GetCategoriesAsync();

		/// <summary>
		/// Add new category
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<ApiResult<string>> AddAsync(RegionDto model);

		/// <summary>
		/// update category
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<ApiResult<string>> UpdateAsync(RegionUpdate model);

		/// <summary>
		/// Delete category
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<ApiResult<string>> DeleteAsync(Guid id);

		/// <summary>
		/// Get object by guid id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<Category?> GetByIdAsync(Guid id);

		/// <summary>
		/// Get all
		/// </summary>
		/// <returns></returns>
		Task<List<Category>> GetAllAsync();

	}
}
