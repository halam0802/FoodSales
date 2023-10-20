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
    public interface ICityService
    {
		Task<ApiResult<List<SelectListModel>>> GetCitiesAsync(Guid regionId);
		Task<ApiResult<string>> AddAsync(CityDto model);
		Task<ApiResult<string>> UpdateAsync(CityUpdate model);
		Task<ApiResult<string>> DeleteAsync(Guid id);
	}
}
