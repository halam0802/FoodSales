using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer
{
	public class CityService: ICityService
	{
		private readonly ICityRepository CityRepository;

		public CityService(ICityRepository CityRepository) 
		{ 
			this.CityRepository = CityRepository;
		}

		public async Task<List<SelectListModel>> GetCitiesAsync()
		{
			var list = await CityRepository.GetAllAsync();

			return list.Select(n => new SelectListModel
			{
				ItemText = n.Name,
				ItemValue = n.Id.ToString()
			}).ToList();
		}

		public async Task<City?> GetByIdAsync(Guid id)
		{
			return await CityRepository.GetByIdAsync(id);
		}

		public async Task<bool> AddAsync(CityDto model)
		{
			var newObj = new City
			{
				Id = Guid.NewGuid(),
				Name = model.Name,
			};
			
			return await CityRepository.AddAsync(newObj);
		}
	
		public async Task<bool> UpdateAsync(CityUpdate model)
		{
			if (model.Id != null)
			{
				var obj = await GetByIdAsync(model.Id.Value);

				if (obj != null)
				{
					obj.Name = model.Name ?? string.Empty;
					obj.UpdatedAt = DateTime.UtcNow;

					return await CityRepository.UpdateAsync(obj);
				}
			}
			
			return false;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			return await CityRepository.DeleteAsync(id);
		}
	}
}