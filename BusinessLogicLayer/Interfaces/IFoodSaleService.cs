using BusinessLogicLayer.Common;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Dto;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IFoodSaleService
    {
		/// <summary>
		/// Get FoodSale pagination list
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		Task<ApiResult<PagingModel<FoodSaleItem>>> FoodSalePaging(FoodSaleFilter request);

		/// <summary>
		/// Get FoodSale detail by guid id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<FoodSaleDetail?> GetDetailAsync(Guid id);

		/// <summary>
		/// Add new FoodSale
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<ApiResult<string>> AddAsync(FoodSaleDto model);

		/// <summary>
		/// update FoodSale
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<ApiResult<string>> UpdateAsync(FoodSaleUpdate model);

		/// <summary>
		/// Delete FoodSale
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<ApiResult<string>> DeleteAsync(Guid id);

		/// <summary>
		/// Delete Many FoodSale
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<ApiResult<string>> DeleteManyAsync(Guid[]? ids);

		/// <summary>
		/// Import file excel
		/// </summary>
		/// <param name="importFile"></param>
		/// <returns><see cref="string"/></returns>
		Task<ApiResult<string>> ImportFileExcel(IFormFile importFile);
	}
}
