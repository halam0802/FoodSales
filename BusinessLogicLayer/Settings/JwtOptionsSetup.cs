using BusinessLogicLayer.JwtToken;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Settings
{
    public class JwtOptionsSetup:IConfigureOptions<JwtOptions>
	{
		private readonly IConfiguration configuration;

		public JwtOptionsSetup(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public void Configure(JwtOptions options)
		{
			configuration.GetSection(nameof(JwtOptions)).Bind(options);
		}
	}

	
}
