using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Models;
using BusinessLogicLayer.JwtToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Interfaces;
using BusinessLogicLayer.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace BusinessLogicLayer.StartUp
{
	public static class StartUpApplication
	{
		public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
		{
			//Add services and repositorys
			RegisterContextService(services);

			//Custom validation response
			CustomValidationResponse(services);

			//Add Jwt
			JwtRegister(services, configuration);

			//Add jwt options
			services.ConfigureOptions<JwtOptionsSetup>();
		}

		public static void ConfigureCors(IApplicationBuilder app, IWebHostEnvironment env)
		{
			//Error missing: IWebHostEnvironment refer to the link
			//https://learn.microsoft.com/en-us/answers/questions/1327368/how-can-i-install-the-nuget-package-microsoft-aspn

			app.UseCors(builder => builder
					 .AllowAnyHeader()
					 .AllowAnyMethod()
					 .SetIsOriginAllowed((host) => true)
					 .AllowCredentials()
				 );
		}


		/// <summary>
		/// Call to configure CORS. <see cref="ConfigureCors"/> need to be call before this.
		/// </summary>
		/// <param name="services"></param>
		public static void AddServices(IServiceCollection services)
		{
			services.AddCors();
		}

		/// <summary>
		/// Custom Validation Response
		/// </summary>
		/// <param name="services">Collection of service descriptors</param>
		private static void CustomValidationResponse(this IServiceCollection services)
		{
			services.Configure<ApiBehaviorOptions>(o =>
			{
				o.InvalidModelStateResponseFactory = actionContext =>
				{
					var Errors = new Dictionary<string, string[]>();

					foreach (var keyModelStatePair in actionContext.ModelState)
					{
						var key = keyModelStatePair.Key;
						var errors = keyModelStatePair.Value.Errors;
						if (errors != null && errors.Count > 0)
						{
							var errorMessages = new string[errors.Count];
							for (var i = 0; i < errors.Count; i++)
							{
								errorMessages[i] = GetErrorMessage(errors[i]);
							}

							Errors.Add(key, errorMessages);
						}
					}
					return new OkObjectResult(ApiResult<Dictionary<string, string[]>>.ValidateModel(null, Errors));
				};
			});
		}

		private static string GetErrorMessage(ModelError error)
		{
			return string.IsNullOrEmpty(error.ErrorMessage) ?
		   "The input was not valid" : (error.ErrorMessage.Contains("required") ? "You have not filled in all the required fields" : error.ErrorMessage);
		}

		/// <summary>
		/// Register Jwt bearer
		/// </summary>
		/// <param name="services"></param>
		/// <param name="configuration"></param>
		/// <exception cref="SecurityTokenExpiredException"></exception>
		private static void JwtRegister(IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => 
					{
				var config = new JwtOptions();
				configuration.GetSection(nameof(JwtOptions)).Bind(config);
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = false,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = config.Issuer,
					ValidAudience = config.Audience,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.SecretKey)),
					ClockSkew = TimeSpan.Zero
				};

				options.Events = new JwtBearerEvents
				{
					OnAuthenticationFailed = async context =>
					{
						if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
						{
							context.Response.Headers.Add("Token-Expired", "true");
							context.Response.StatusCode = 401;
							var msg = context.Exception.Message;
							context.Exception = null;
							await context.HttpContext.Response.WriteAsync($"{{\"success\":false,\"error\":\"{msg}\",\"errorCode\":400,\"content\":null}}");
						}
					},
					OnChallenge = context =>
					{
						if ((string.IsNullOrEmpty(context.Error) || context.Error == "invalid_token") && context.Response.StatusCode != 401)
						{
							context.Response.StatusCode = 401;
						}

						context.HandleResponse();
						return Task.CompletedTask;
					},
					OnTokenValidated = async context =>
					{
						try
						{
							if (config.Enabled)
							{
								var jwtAuthentication = context.HttpContext.RequestServices.GetRequiredService<IJwtBearerUserAuthenticationService>();
								var isValid = await jwtAuthentication.Valid(context);
								if (!isValid)
									throw new SecurityTokenExpiredException();
							}
							else
								throw new SecurityTokenExpiredException("API is disable");
						}
						catch (Exception ex)
						{
							throw new SecurityTokenExpiredException(ex.Message);
						}
					},
				};
			});
		}

		/// <summary>
		/// Register context service
		/// </summary>
		/// <param name="services"></param>
		private static void RegisterContextService(IServiceCollection services)
		{
			#region Repository
			services.AddScoped<IRegionRepository, RegionRepository>();
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<IProductRepository, ProductRepository>();
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<IFoodSaleRepository, FoodSaleRepository>();
			services.AddScoped<ICityRepository, CityRepository>();
			services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
			#endregion

			#region Services

			services.AddScoped<IRegionService, RegionService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IFoodSaleService, FoodSaleService>();
			services.AddScoped<ICityService, CityService>();
			services.AddScoped<IRefreshTokenService, RefreshTokenService>();
			services.AddScoped<IJwtBearerUserAuthenticationService, JwtBearerUserAuthenticationService>();
			#endregion
		}

	}
}
