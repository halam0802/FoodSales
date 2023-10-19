using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.JwtToken;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer
{
	public class UserService: IUserService
	{
		private readonly IUserRepository UserRepository;
		private readonly IJwtService jwtService;
		public UserService(IUserRepository UserRepository, IJwtService jwtService) 
		{ 
			this.UserRepository = UserRepository;
			this.jwtService = jwtService;
		}

		public async Task<ApiResult<LoginResult>> Authenticate(string userName, string password)
		{
			try
			{
				var user = await UserRepository.GetUserByUsernameAsync(userName);

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

					return ApiResult<LoginResult>.Successfully(result);
				}
			}
			catch (Exception ex)
			{
				return ApiResult<LoginResult>.Failure("LOGIN: " + ex.Message);
			}

			return ApiResult<LoginResult>.Failure();
		}

		public async Task<List<SelectListModel>> GetUsersAsync()
		{
			var list = await UserRepository.GetAllAsync();

			return list.Select(n => new SelectListModel
			{
				ItemText = n.Name,
				ItemValue = n.Id.ToString()
			}).ToList();
		}

		public async Task<User?> GetByIdAsync(Guid id)
		{
			return await UserRepository.GetByIdAsync(id);
		}

		public async Task<bool> AddAsync(UserDto model)
		{
			var newObj = new User
			{
				Id = Guid.NewGuid(),
				Name = model.Name,
			};
			
			return await UserRepository.AddAsync(newObj);
		}
	
		public async Task<bool> UpdateAsync(UserUpdate model)
		{
			if (model.Id != null)
			{
				var obj = await GetByIdAsync(model.Id.Value);

				if (obj != null)
				{
					obj.Name = model.Name ?? string.Empty;
					obj.UpdatedAt = DateTime.UtcNow;

					return await UserRepository.UpdateAsync(obj);
				}
			}
			
			return false;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			return await UserRepository.DeleteAsync(id);
		}

		public string GetPasswordHash(string password, string passwordSalt)
		{
			return EncryptionHelper.CreatePasswordHash(password, passwordSalt);
		}
	}
}