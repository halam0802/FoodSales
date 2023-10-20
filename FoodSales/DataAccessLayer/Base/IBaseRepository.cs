using DataAccessLayer.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Base
{
    public interface IBaseRepository<T>
    {
        IEnumerable<T> Table { get; }

		Task<MessageReport> AddAsync(T model);

        Task<MessageReport> UpdateAsync(T model);

        Task<MessageReport> DeleteAsync(T model);

		Task<T?> GetByIdAsync(Guid id);
	}
}
