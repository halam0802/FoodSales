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
    public interface IProductService
    {
		/// <summary>
		/// Get product list by category id
		/// </summary>
		/// <returns></returns>
		Task<ApiResult<List<SelectListModel>>> GetProductsAsync(Guid? categoryId);

		/// <summary>
		/// Add new product
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<ApiResult<string>> AddAsync(ProductDto model);

		/// <summary>
		/// update product
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		Task<ApiResult<string>> UpdateAsync(ProductUpdate model);

		/// <summary>
		/// Delete product
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<ApiResult<string>> DeleteAsync(Guid id);

		/// <summary>
		/// Get object by guid id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<Product?> GetByIdAsync(Guid id);

		/// <summary>
		/// Get all
		/// </summary>
		/// <returns></returns>
		Task<List<Product>> GetAllAsync();
	}
}
