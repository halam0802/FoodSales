using BusinessLogicLayer.Models;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface IOrderService
    {
        Task<List<SelectListModel>> GetOrdersAsync();
        Task<Order?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(OrderDto model);
        Task<bool> UpdateAsync(OrderUpdate model);
        Task<bool> DeleteAsync(Guid id);
    }
}
