using BusinessLogicLayer.Models.Dto;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.JwtToken
{
	public class RefreshTokenService : IRefreshTokenService
	{
		private readonly JwtOptions options;
		private readonly IRefreshTokenRepository _refreshTokenRepository;
		public RefreshTokenService(IOptions<JwtOptions> options, IRefreshTokenRepository _refreshTokenRepository)
		{
			this.options = options.Value;
			this._refreshTokenRepository = _refreshTokenRepository;
		}

		public async Task<TokenDto> GenerateToken(User user)
		{
			var refreshTokenValue = GenerateRefreshToken();

			var objRefreshToken = await SaveRefreshTokenToUser(user, refreshTokenValue);

			var claims = new Claim[]
			{
				new (JwtRegisteredClaimNames.Sub,user.Id.ToString()),
				new (JwtRegisteredClaimNames.Email,user.Username),
				new (JwtRegisteredClaimNames.Name,user.Name),
				new ("RefreshKey",objRefreshToken.Key),
			};

			var signingCredentials = new SigningCredentials(
				new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.SecretKey))
				, SecurityAlgorithms.HmacSha256
				);

			var token = new JwtSecurityToken(
				options.Issuer,
				options.Audience,
				claims,
				null,
				DateTime.UtcNow.AddMinutes(options.ExpiryInMinutes),
				signingCredentials
				);

			string tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

			return new TokenDto
			{
				AccessToken = tokenValue,
				RefreshToken = refreshTokenValue
			};
		}

		public string GenerateRefreshToken()
		{
			var randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		public async Task<RefreshToken?> SaveRefreshTokenToUser(User user, string refreshToken)
		{
			var obj = await GetUserRefreshToken(user.Id);

			if (obj == null)
			{
				obj = new RefreshToken
				{
					Id = Guid.NewGuid(),
					UserId = user.Id,
					Token = refreshToken,
					Key = Guid.NewGuid().ToString(),
					ValidTo = DateTime.UtcNow.AddMinutes(options.RefreshTokenExpiryInMinutes)
				};

				await _refreshTokenRepository.AddAsync(obj);
			}
			else
			{
				obj.Token = refreshToken;
				obj.Key = Guid.NewGuid().ToString();
				obj.UpdatedAt = DateTime.UtcNow;
				obj.ValidTo = DateTime.UtcNow.AddMinutes(options.RefreshTokenExpiryInMinutes);

				await _refreshTokenRepository.UpdateAsync(obj);
			}

			return obj;
		}

		public async Task<RefreshToken?> GetUserRefreshToken(Guid userId)
		{
			var obj = _refreshTokenRepository.Table.OrderByDescending(n => n.CreatedAt).FirstOrDefault(n => n.UserId == userId && !n.Deleted);

			return await Task.FromResult(obj);
		}
		public async Task<RefreshToken?> GetOneByToken(string refreshToken)
		{
			var obj = _refreshTokenRepository.Table.FirstOrDefault(n => n.Token == refreshToken && !n.Deleted);

			return await Task.FromResult(obj);
		}
		//public ClaimsPrincipal GetPrincipalFromToken(string token)
		//{
		//	var tokenValidationParameters = new TokenValidationParameters
		//	{
		//		ValidateAudience = false,
		//		ValidateIssuer = true,
		//		ValidateIssuerSigningKey = true,
		//		IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.SecretKey)),
		//		ValidateLifetime = true
		//	};

		//	var tokenHandler = new JwtSecurityTokenHandler();
		//	var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
		//	if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
		//		throw new SecurityTokenException("Invalid token");

		//	return principal;
		//}
	}
}
