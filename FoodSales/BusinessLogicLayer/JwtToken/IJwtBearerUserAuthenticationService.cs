using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.JwtToken
{
	public interface IJwtBearerUserAuthenticationService
	{
		Task<bool> Valid(TokenValidatedContext context);
	}
}
