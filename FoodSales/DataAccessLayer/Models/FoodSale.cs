using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccessLayer.Models
{
	public class FoodSale:BaseEntity
	{
		public Guid ProductId { get; set; }

		public Guid CategoryId { get; set; }

		public Guid RegionId { get; set; }

		public Guid CityId { get; set; }

		[StringLength(150)]
		public string? ProductName { get; set; }

		[StringLength(50)]
		public string? CategoryName { get; set; }

		[StringLength(50)]
		public string? RegionName { get; set; }

		[StringLength(50)]
		public string? CityName { get; set; }

		public int Quantity { get; set; }

		public decimal UnitPrice { get; set; }

		public decimal TotalPrice { get; set; }
	}
}
