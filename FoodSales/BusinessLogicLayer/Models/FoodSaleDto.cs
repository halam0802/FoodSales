using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
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
	public class OrderUpdate : FoodSaleDto
	{
		public Guid? Id { get; set; }
	}
}
