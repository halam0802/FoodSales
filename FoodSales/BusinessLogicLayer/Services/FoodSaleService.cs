using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer
{
    public class FoodSaleService: IFoodSaleService
	{
		private readonly IFoodSaleRepository _foodSaleRepository;

		public FoodSaleService(IFoodSaleRepository _foodSaleRepository) 
		{ 
			this._foodSaleRepository = _foodSaleRepository;
		}

		public async Task<ApiResult<string>> AddAsync(FoodSaleDto model)
		{
			try
			{

				var newObj = new FoodSale
				{
					Id = Guid.NewGuid(),
					Name = model.Name,
				};

				var result = await _cityRepository.AddAsync(newObj);

				if (!result.isSuccess)
					return ApiResult<string>.Failure(result.Message);

				return ApiResult<string>.Successfully(newObj.Name);
			}
			catch (Exception ex)
			{
				_logger.LogError($"CityAddAsync ERROR: {ex.Message}");

				return ApiResult<string>.ErrorInProcessing(ex.Message);
			}
		}
	}
}