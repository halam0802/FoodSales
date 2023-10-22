using BusinessLogicLayer.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Common
{
	/// <summary>
	/// Paged list
	/// </summary>
	/// <typeparam name="T">T</typeparam>
	[Serializable]
	public partial class PagedList<T> : List<T>, IPagedList<T>
	{

		private void Initialize(IEnumerable<T> source, int pageIndex, int pageSize, int? totalCount = null)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			if (pageSize <= 0)
				pageSize = 1;

			TotalCount = totalCount ?? source.Count();

			if (pageSize > 0)
			{
				TotalPages = TotalCount / pageSize;
				if (TotalCount % pageSize > 0)
					TotalPages++;
			}

			PageSize = pageSize;
			PageIndex = pageIndex;
			source = totalCount == null ? source.Skip(pageIndex * pageSize).Take(pageSize) : source;
			AddRange(source);
		}

		public PagedList()
		{
		}
		public PagedList(IEnumerable<T> source, int pageIndex, int pageSize)
		{
			Initialize(source, pageIndex, pageSize);
		}

		public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
		{
			Initialize(source, pageIndex, pageSize, totalCount);
		}

		private Task InitializeAsync(IQueryable<T> source, int pageIndex, int pageSize, int? totalCount = null)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			if (pageSize <= 0)
				pageSize = 10;

			if (pageIndex <= 0)
				pageIndex = 1;

			TotalCount = totalCount ?? source.Count();
			
			if (totalCount == null)
			{
				source = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
			}
			AddRange(source);

			if (pageSize > 0)
			{
				TotalPages = TotalCount / pageSize;
				if (TotalCount % pageSize > 0)
					TotalPages++;
			}

			PageSize = pageSize;
			PageIndex = pageIndex;
			return Task.CompletedTask;
		}

		public static async Task<PagedList<T>> Create(IQueryable<T> source, int pageIndex, int pageSize)
		{
			var pagelist = new PagedList<T>();
			await pagelist.InitializeAsync(source, pageIndex, pageSize);
			return pagelist;
		}

		public static async Task<PagedList<T>> PagingList(IQueryable<T> source, int pageIndex, int pageSize)
		{
			var pagelist = new PagedList<T>();
			await pagelist.InitializeAsync(source, pageIndex, pageSize);
			return new PagedList<T>
			{
				PageIndex = pagelist.PageIndex,
				PageSize = pagelist.PageSize,
				TotalCount = pagelist.TotalCount,
				TotalPages = pagelist.TotalPages,
				Data = pagelist
			};
		}

		public List<T> Data { get; protected set; }
		public int PageIndex { get; protected set; }
		public int PageSize { get; protected set; }
		public int TotalCount { get; protected set; }
		public int TotalPages { get; protected set; }

		public bool HasPreviousPage
		{
			get { return (PageIndex > 0); }
		}
		public bool HasNextPage
		{
			get { return (PageIndex + 1 < TotalPages); }
		}
	}
}
