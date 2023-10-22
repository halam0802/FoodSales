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
    public interface IRegionService
    {
		/// <summary>
		/// Get region list
		/// </summary>
		/// <returns></returns>
		Task<ApiResult<List<SelectListModel>>> GetRegionsAsync();

		/// <summary>
		/// Add new region
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<ApiResult<string>> AddAsync(RegionDto model);

		/// <summary>
		/// update region
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<ApiResult<string>> UpdateAsync(RegionUpdate model);

		/// <summary>
		/// Delete region
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<ApiResult<string>> DeleteAsync(Guid id);

		/// <summary>
		/// Get object by guid id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<Region?> GetByIdAsync(Guid id);

		/// <summary>
		/// Get all
		/// </summary>
		/// <returns></returns>
		Task<List<Region>> GetAllAsync();
	}
}
