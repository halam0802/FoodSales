using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodSales.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
		public UserController(IUserService _userService)
		{
			this._userService = _userService;
		}

		/// <summary>
		/// User login
		/// </summary>
		/// <param name="request"><see cref="AuthenticateDto"/></param>
		/// <returns><see cref="LoginResult"/></returns>
		[AllowAnonymous]
		[HttpPost(nameof(Login))]
		public async Task<ApiResult<LoginResult>> Login(AuthenticateDto request)
		{
			return await _userService.Authenticate(request.Username, request.Password);
		}

		/// <summary>
		/// Get access token by refresh token
		/// </summary>
		/// <param name="refreshToken"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet(nameof(GetAccessTokenByRefreshToken))]
		public async Task<ApiResult<string>> GetAccessTokenByRefreshToken(string refreshToken)
		{
			return await _userService.GetAccessTokenByRefreshToken(refreshToken);
		}
	}
}
