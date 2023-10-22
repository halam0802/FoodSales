using BusinessLogicLayer;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.JwtToken;
using BusinessLogicLayer.Services;
using BusinessLogicLayer.Settings;
using BusinessLogicLayer.StartUp;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var conn = builder.Configuration.GetConnectionString("FoodSaleDatabase");

builder.Services.AddDbContext<DataContext>(options =>
		options.UseSqlServer(conn));

//using (var context = new DataContext(conn ?? string.Empty))
//{
//	if (context.Database.GetPendingMigrations().Count() > 0)
//		context.Database.Migrate();
//}

//Add mandatory services
StartUpApplication.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

//Add cors
StartUpApplication.ConfigureCors(app, app.Environment);
StartUpApplication.AddServices(builder.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
