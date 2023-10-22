using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.JwtToken;
using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer
{
    public class UserService: IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IRefreshTokenService _refreshTokenService;

		public UserService(IUserRepository _userRepository, IRefreshTokenService _refreshTokenService) 
		{ 
			this._userRepository = _userRepository;
			this._refreshTokenService = _refreshTokenService;
			
		}

		/// <summary>
		/// User login
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		public async Task<ApiResult<LoginResult>> Authenticate(string userName, string password)
		{
			try
			{
				var user = _userRepository.Table.FirstOrDefault(n => !n.Deleted && n.Username == userName);

				if (user != null && user.Password == GetPasswordHash(password, user.PasswordSalt))
				{
					var objToken = await _refreshTokenService.GenerateToken(user);

					var result = new LoginResult
					{
						Id = user.Id,
						Name = user.Name,
						Username = user.Username,
						Token = objToken.AccessToken,
						RefreshToken = objToken.RefreshToken,
					};

					return ApiResult<LoginResult>.Successfully(result);
				}

				return ApiResult<LoginResult>.Failure();
			}
			catch (Exception ex)
			{
				return ApiResult<LoginResult>.Failure("LOGIN: " + ex.Message);
			}
		}

		/// <summary>
		/// Get access token by refresh token
		/// </summary>
		/// <param name="refreshToken"></param>
		/// <returns></returns>
		public async Task<ApiResult<string>> GetAccessTokenByRefreshToken(string refreshToken)
		{
			try
			{
				var objRefreshToken = await _refreshTokenService.GetOneByToken(refreshToken);

				if(objRefreshToken != null && objRefreshToken.ValidTo > DateTime.UtcNow)
				{
					var user = await GetByIdAsync(objRefreshToken.UserId);

					if(user != null)
					{
						var objToken = await _refreshTokenService.GenerateToken(user);

						var token = objToken != null ? objToken.AccessToken : "";

						return ApiResult<string>.Successfully(token);
					}					
				}
				
				return ApiResult<string>.Failure();
			}
			catch (Exception ex)
			{
				return ApiResult<string>.Failure("GetTokenByRefreshToken: " + ex.Message);
			}
		}

		/// <summary>
		/// Get object by guid id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<User?> GetByIdAsync(Guid id)
		{
			return await _userRepository.GetByIdAsync(id);
		}

		public string GetPasswordHash(string password, string passwordSalt)
		{
			return EncryptionHelper.CreatePasswordHash(password, passwordSalt);
		}
	}
}