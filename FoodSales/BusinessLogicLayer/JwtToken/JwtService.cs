using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLogicLayer.JwtToken
{
    public class JwtService : IJwtService
	{
		private readonly JwtOptions options;
		public JwtService(IOptions<JwtOptions> options)
		{
			this.options = options.Value;
		}
		public string GenerateToken(User user)
		{
			var claims = new Claim[] 
			{
				new (JwtRegisteredClaimNames.Sub,user.Id.ToString()),
				new (JwtRegisteredClaimNames.Email,user.Username),
				new (JwtRegisteredClaimNames.Name,user.Name),
			};

			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey))
				,SecurityAlgorithms.HmacSha256
				);

			var token = new JwtSecurityToken(
				options.Issuer,
				options.Audience,
				claims,
				null,
				DateTime.UtcNow.AddHours(12),
				signingCredentials
				);

			string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

			return tokenValue;
		}
	}
}
