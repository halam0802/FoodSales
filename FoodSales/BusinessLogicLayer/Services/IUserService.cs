
using BusinessLogicLayer.Models;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface IUserService
    {
        Task<List<SelectListModel>> GetUsersAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(UserDto model);
        Task<bool> UpdateAsync(UserUpdate model);
        Task<bool> DeleteAsync(Guid id);

        Task<ApiResult<LoginResult>> Authenticate(string userName, string password);
	}
}
