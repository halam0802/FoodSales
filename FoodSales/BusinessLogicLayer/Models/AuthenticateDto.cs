using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
	public class AuthenticateDto
	{
		[DisplayName("Username")]
		[Required(ErrorMessage = "{0} is required")]
		public string Username { get; set; }
		[DisplayName("Password")]
		[Required(ErrorMessage = "{0} is required")]
		public string Password { get; set; }
	}
}
