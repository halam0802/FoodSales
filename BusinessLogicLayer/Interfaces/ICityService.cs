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
    public interface ICityService
    {
		/// <summary>
		/// Get city list by region id
		/// </summary>
		/// <returns></returns>
		Task<ApiResult<List<SelectListModel>>> GetCitiesAsync(Guid? regionId);

		/// <summary>
		/// Add new city
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<ApiResult<string>> AddAsync(CityDto model);

		/// <summary>
		/// update city
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<ApiResult<string>> UpdateAsync(CityUpdate model);

		/// <summary>
		/// Delete city
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<ApiResult<string>> DeleteAsync(Guid id);

		/// <summary>
		/// Get object by guid id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<City?> GetByIdAsync(Guid id);

		/// <summary>
		/// Get all
		/// </summary>
		/// <returns></returns>
		Task<List<City>> GetAllAsync();
	}
}
