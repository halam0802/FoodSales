using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models.Dto
{
    public class FoodSaleDto
    {
        public Guid ProductId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid RegionId { get; set; }

        public Guid CityId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }
    }
    public class FoodSaleUpdate : FoodSaleDto
    {
        public Guid Id { get; set; }
    }

	public class FoodSaleDeleteMany
	{
		public Guid[]? Ids { get; set; }
	}

	public class FoodSaleDetail : FoodSaleUpdate
	{
		public decimal TotalPrice { get; set; }
	}

	public class FoodSaleItem
	{
		public Guid Id { get; set; }

		public string? ProductName { get; set; }

		public string? CategoryName { get; set; }

		public string? RegionName { get; set; }

		public string? CityName { get; set; }

		public int Quantity { get; set; }

		public decimal UnitPrice { get; set; }

		public decimal TotalPrice { get; set; }

		public DateTime CreateAt { get; set; }
	}

	public class FoodSaleFilter
	{
		public string? ProductName { get; set; }

		public string? CategoryName { get; set; }

		public string? RegionName { get; set; }

		public string? CityName { get; set; }

		public int Quantity { get; set; }

		public decimal UnitPrice { get; set; }

		public decimal TotalPrice { get; set; }

		/// <summary>
		/// format yyyy/MM/dd
		/// </summary>
		public string? FromDate { get; set; }

		/// <summary>
		/// format yyyy/MM/dd
		/// </summary>
		public string? ToDate { get; set; }

		public string? SortDate { get; set; }

		public string? SortRegion { get; set; }

		public string? SortCity { get; set; }

		public string? SortCategory { get; set; }
		public string? SortProduct { get; set; }
		public string? SortQuantity { get; set; }
		public string? SortUnitPrice { get; set; }
		public string? SortTotalPrice { get; set; }

		public int PageIndex { get; set; }

		public int PageSize { get; set; }
	}
}
