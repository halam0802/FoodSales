using DataAccessLayer.Models;
using DataAccessLayer.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Base
{
	public abstract class BaseRepository<T> where T : class
	{
		private DataContext dataContext;
		private readonly DbSet<T> dbset;

		public BaseRepository(DbContextOptions<DataContext> options)
		{
			dataContext = dataContext ?? (dataContext = new DataContext(options));
			dbset = dataContext.Set<T>();
		}

		public async Task<MessageReport> AddAsync(T model)
		{
			var result = new MessageReport(false, "ERROR!");

			try
			{
				await dbset.AddAsync(model);

				await dataContext.SaveChangesAsync();

				result = new MessageReport(true, "OK");
			}
			catch (Exception ex)
			{
				result = new MessageReport(false, string.Format("Message: {0} - Details: {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : ""));
			}

			return result;
		}

		public async Task<MessageReport> DeleteAsync(T model)
		{
			var result = new MessageReport(false, "ERROR!");

			try
			{
				dbset.Remove(model);

				await dataContext.SaveChangesAsync();

				result = new MessageReport(true, "OK");
			}
			catch (System.Exception ex)
			{
				result = new MessageReport(false, string.Format("Message: {0} - Details: {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : ""));
			}

			return result;
		}

		public async Task<MessageReport> UpdateAsync(T model)
		{
			var result = new MessageReport(false, "ERROR!");

			try
			{
				dbset.Attach(model);

				dataContext.Entry(model).State = EntityState.Modified;

				await dataContext.SaveChangesAsync();

				result = new MessageReport(true, "OK");
			}
			catch (System.Exception ex)
			{
				result = new MessageReport(false, string.Format("Message: {0} - Details: {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : ""));
			}

			return result;
		}

		public async Task<T?> GetByIdAsync(Guid id)
		{
			return await dbset.FindAsync(id);
		}

		public virtual IEnumerable<T> Table
		{
			get
			{
				return dbset;
			}
		}
	}
}
