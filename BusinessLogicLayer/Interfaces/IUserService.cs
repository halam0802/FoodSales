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
        /// <summary>
        /// User login
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<ApiResult<LoginResult>> Authenticate(string userName, string password);

		/// <summary>
		/// Get access token by refresh token
		/// </summary>
		/// <param name="refreshToken"></param>
		/// <returns></returns>
		Task<ApiResult<string>> GetAccessTokenByRefreshToken(string refreshToken);

		/// <summary>
		/// Get object by guid id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<User?> GetByIdAsync(Guid id);

	}
}
