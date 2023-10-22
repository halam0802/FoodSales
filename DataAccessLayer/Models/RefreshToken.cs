using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
	public class RefreshToken:BaseEntity
	{
		public Guid UserId { get; set; }
		public string Key { get; set; }
		public string Token { get; set; }
		public DateTime ValidTo { get; set; }
	}
}
