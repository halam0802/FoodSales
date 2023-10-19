using BusinessLogicLayer.Model;
using BusinessLogicLayer.Services;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer
{
	public class UserService: IUserService
	{
		private readonly IUserRepository UserRepository;

		public UserService(IUserRepository UserRepository) 
		{ 
			this.UserRepository = UserRepository;
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
	}
}