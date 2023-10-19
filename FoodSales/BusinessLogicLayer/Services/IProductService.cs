using BusinessLogicLayer.Model;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface IProductService
    {
        Task<List<SelectListModel>> GetProductsAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(ProductDto model);
        Task<bool> UpdateAsync(ProductUpdate model);
        Task<bool> DeleteAsync(Guid id);
    }
}
