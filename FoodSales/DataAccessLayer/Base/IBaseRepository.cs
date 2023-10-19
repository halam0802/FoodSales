using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Base
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> GetAllAsync();

        Task<bool> AddAsync(T model);

        Task<bool> UpdateAsync(T model);

        Task<bool> DeleteAsync(Guid id);

		Task<T?> GetByIdAsync(Guid id);
	}
}
