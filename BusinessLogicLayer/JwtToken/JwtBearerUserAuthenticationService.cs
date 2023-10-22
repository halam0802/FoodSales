using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.JwtToken
{
	public class JwtBearerUserAuthenticationService : IJwtBearerUserAuthenticationService
	{
		private readonly IUserService _userService;
		private readonly IRefreshTokenService _refreshTokenService;
		public JwtBearerUserAuthenticationService(IUserService _userService, IRefreshTokenService _refreshTokenService)
		{
			this._userService = _userService;
			this._refreshTokenService = _refreshTokenService;
		}
		public async Task<bool> Valid(TokenValidatedContext context)
		{
			try
			{
				if (context.Principal == null) return false;
				var id = context.Principal.Claims.ToList().FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
				var userName = context.Principal.Claims.ToList().FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
				var name = context.Principal.Claims.ToList().FirstOrDefault(x => x.Type == "name")?.Value;
				var refreshKey = context.Principal.Claims.ToList().FirstOrDefault(x => x.Type == "RefreshKey")?.Value;

				if (!string.IsNullOrEmpty(id))
				{
					var refreshToken = await _refreshTokenService.GetUserRefreshToken(Guid.Parse(id));
					if (refreshToken is null || string.IsNullOrEmpty(refreshKey) || !refreshKey.Equals(refreshToken.Key) || refreshToken.ValidTo <= DateTime.UtcNow)
					{
						return false;
					}

					var user = await _userService.GetByIdAsync(Guid.Parse(id));

					if (user != null && user.Name == name && user.Username == userName)
						return true;
				}
			}
			catch (Exception)
			{
			}

			return false;
		}
	}
}
