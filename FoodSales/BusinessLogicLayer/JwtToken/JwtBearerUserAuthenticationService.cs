using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
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

		public JwtBearerUserAuthenticationService(IUserService _userService)
		{
			this._userService = _userService;

		}
		public async Task<bool> Valid(TokenValidatedContext context)
		{
			if (context.Principal == null) return false;
			var id = context.Principal.Claims.ToList().FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
			var userName = context.Principal.Claims.ToList().FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
			var name = context.Principal.Claims.ToList().FirstOrDefault(x => x.Type == "name")?.Value;

			if (!string.IsNullOrEmpty(id))
			{
				var user = await _userService.GetByIdAsync(Guid.Parse(id));

				if (user != null && user.Name == name && user.Username == userName)
					return true;
			}
			
			return false;
		}
	}
}
