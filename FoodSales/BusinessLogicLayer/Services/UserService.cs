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
		private readonly IJwtService jwtService;
		public UserService(IUserRepository _userRepository, IJwtService jwtService) 
		{ 
			this._userRepository = _userRepository;
			this.jwtService = jwtService;
		}

		public async Task<ApiResult<LoginResult>> Authenticate(string userName, string password)
		{
			try
			{
				var user = _userRepository.Table.FirstOrDefault(n => !n.Deleted && n.Username == userName);

				if (user != null && user.Password == GetPasswordHash(password, user.PasswordSalt))
				{
					var token = jwtService.GenerateToken(user);

					var result = new LoginResult
					{
						Id = user.Id,
						Name = user.Name,
						Username = user.Username,
						Token = token,
					};

					return await Task.FromResult(ApiResult<LoginResult>.Successfully(result));
				}

				return await Task.FromResult(ApiResult<LoginResult>.Failure());
			}
			catch (Exception ex)
			{
				return await Task.FromResult(ApiResult<LoginResult>.Failure("LOGIN: " + ex.Message));
			}
		}

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