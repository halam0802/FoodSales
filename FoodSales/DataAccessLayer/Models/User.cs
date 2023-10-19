using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
	public class User : BaseEntity
	{
		[StringLength(150)]
		public string Name { get; set; }

		[StringLength(50)]
		public string Username { get; set; }

		[StringLength(50)]
		public string? UsernameNormalize { get; set; }
		[StringLength(300)]
		public string? Password { get; set; }
		[StringLength(10)]
		public string? PasswordSalt { get; set; }
		[StringLength(300)]
		public string? PasswordToken { get; set; }

	}
}
