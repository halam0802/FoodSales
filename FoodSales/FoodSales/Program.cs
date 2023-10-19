using BusinessLogicLayer;
using BusinessLogicLayer.Services;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

builder.Services.AddScoped<IRegionService, RegionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICityService, CityService>();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
