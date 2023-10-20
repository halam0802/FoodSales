using BusinessLogicLayer.Models;
using DataAccessLayer.Models;
using DataAccessLayer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        Task<ApiResult<LoginResult>> Authenticate(string userName, string password);
        Task<User?> GetByIdAsync(Guid id);

	}
}
