using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.JwtToken
{
	public interface IJwtService
	{
		string GenerateToken(User user);
	}
}
