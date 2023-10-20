using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
	public class LoginResult
	{
		public Guid Id { get; set; }
		public string? Name { get; set; }
		public string? Username { get; set; }
		public string? Token { get; set; }
	}
}
