using BusinessLogicLayer.Models;
using BusinessLogicLayer.Services;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer
{
	public class RegionService: IRegionService
	{
		private readonly IRegionRepository regionRepository;

		public RegionService(IRegionRepository regionRepository) 
		{ 
			this.regionRepository = regionRepository;
		}

		public async Task<List<SelectListModel>> GetRegionsAsync()
		{
			var list = await regionRepository.GetAllAsync();

			return list.Select(n => new SelectListModel
			{
				ItemText = n.Name,
				ItemValue = n.Id.ToString()
			}).ToList();
		}

		public async Task<Region?> GetByIdAsync(Guid id)
		{
			return await regionRepository.GetByIdAsync(id);
		}

		public async Task<bool> AddAsync(RegionDto model)
		{
			var newObj = new Region
			{
				Id = Guid.NewGuid(),
				Name = model.Name,
			};
			
			return await regionRepository.AddAsync(newObj);
		}
	
		public async Task<bool> UpdateAsync(RegionUpdate model)
		{
			if (model.Id != null)
			{
				var obj = await GetByIdAsync(model.Id.Value);

				if (obj != null)
				{
					obj.Name = model.Name ?? string.Empty;
					obj.UpdatedAt = DateTime.UtcNow;

					return await regionRepository.UpdateAsync(obj);
				}
			}
			
			return false;
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			return await regionRepository.DeleteAsync(id);
		}
	}
}