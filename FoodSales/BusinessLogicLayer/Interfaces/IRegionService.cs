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
    public interface IRegionService
    {
		Task<ApiResult<List<SelectListModel>>> GetRegionsAsync();
		Task<ApiResult<string>> AddAsync(RegionDto model);
		Task<ApiResult<string>> UpdateAsync(RegionUpdate model);
		Task<ApiResult<string>> DeleteAsync(Guid id);
	}
}
