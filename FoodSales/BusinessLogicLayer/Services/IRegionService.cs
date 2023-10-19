using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface IRegionService
    {
        Task<List<SelectListModel>> GetRegionsAsync();
        Task<Region?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(RegionDto model);
        Task<bool> UpdateAsync(RegionUpdate model);
        Task<bool> DeleteAsync(Guid id);
    }
}
