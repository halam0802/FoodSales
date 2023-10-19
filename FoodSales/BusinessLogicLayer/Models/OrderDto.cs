using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
	public class OrderDto
	{
	}
	public class OrderUpdate : OrderDto
	{
		public Guid? Id { get; set; }
	}
}
