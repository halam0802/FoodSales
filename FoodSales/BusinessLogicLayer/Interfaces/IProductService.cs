using BusinessLogicLayer.Models;
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
		Task<ApiResult<List<SelectListModel>>> GetProductsAsync(Guid categoryId);
		Task<ApiResult<string>> AddAsync(ProductDto model);
		Task<ApiResult<string>> UpdateAsync(ProductUpdate model);
		Task<ApiResult<string>> DeleteAsync(Guid id);
	}
}
