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
    public interface ICityService
    {
        Task<List<SelectListModel>> GetCitiesAsync();
        Task<City?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(CityDto model);
        Task<bool> UpdateAsync(CityUpdate model);
        Task<bool> DeleteAsync(Guid id);
    }
}
