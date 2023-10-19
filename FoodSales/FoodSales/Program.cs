using BusinessLogicLayer;
using BusinessLogicLayer.JwtToken;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Settings;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();

#region Repository
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
#endregion

#region Services

builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IJwtBearerUserAuthenticationService, JwtBearerUserAuthenticationService>();
#endregion

#region Custom Validation Response
builder.Services.Configure<ApiBehaviorOptions>(o =>
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
		return new OkObjectResult(Errors);
	};
});

static string GetErrorMessage(ModelError error)
{
	return string.IsNullOrEmpty(error.ErrorMessage) ?
   "The input was not valid" : (error.ErrorMessage.Contains("required") ? "You have not filled in all the required fields" : error.ErrorMessage);
}
#endregion

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
	{
		var config = new JwtOptions();
		builder.Configuration.GetSection(nameof(JwtOptions)).Bind(config);
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = false,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = config.Issuer,
			ValidAudience = config.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.SecretKey)),
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
				if (string.IsNullOrEmpty(context.Error) || context.Error == "invalid_token")
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

builder.Services.ConfigureOptions<JwtOptionsSetup>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
