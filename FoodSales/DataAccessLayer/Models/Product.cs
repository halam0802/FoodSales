using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
	public class Product: BaseEntity
	{
		[StringLength(150)]
		public string Name { get; set; }
		public Guid CategoryId { get; set; }
	}
}
