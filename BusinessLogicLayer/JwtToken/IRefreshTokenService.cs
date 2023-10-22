using BusinessLogicLayer.Models.Dto;
using DataAccessLayer.Models;
using DataAccessLayer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.JwtToken
{
	public interface IRefreshTokenService
	{
		string GenerateRefreshToken();
		Task<RefreshToken?> SaveRefreshTokenToUser(User user, string refreshToken);
		Task<RefreshToken?> GetUserRefreshToken(Guid userId);

		Task<RefreshToken?> GetOneByToken(string refreshToken);
		Task<TokenDto> GenerateToken(User user);
	}
}
