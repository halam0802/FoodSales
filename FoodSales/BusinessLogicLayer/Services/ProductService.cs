using BusinessLogicLayer.Model;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository ProductRepository;

        public ProductService(IProductRepository ProductRepository)
        {
            this.ProductRepository = ProductRepository;
        }

        public async Task<List<SelectListModel>> GetProductsAsync()
        {
            var list = await ProductRepository.GetAllAsync();

            return list.Select(n => new SelectListModel
            {
                ItemText = n.Name,
                ItemValue = n.Id.ToString()
            }).ToList();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            return await ProductRepository.GetByIdAsync(id);
        }

        public async Task<bool> AddAsync(ProductDto model)
        {
            var newObj = new Product
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
            };

            return await ProductRepository.AddAsync(newObj);
        }

        public async Task<bool> UpdateAsync(ProductUpdate model)
        {
            if (model.Id != null)
            {
                var obj = await GetByIdAsync(model.Id.Value);

                if (obj != null)
                {
                    obj.Name = model.Name ?? string.Empty;
                    obj.UpdatedAt = DateTime.UtcNow;

                    return await ProductRepository.UpdateAsync(obj);
                }
            }

            return false;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await ProductRepository.DeleteAsync(id);
        }
    }
}