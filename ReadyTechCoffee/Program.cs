
using Microsoft.EntityFrameworkCore;
using ReadyTechCoffee.Business.BusinessObjects;
using ReadyTechCoffee.Business.Services;
using ReadyTechCoffee.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ICoffeeRespository, CoffeeRepository>();
builder.Services.AddScoped<IDate,Date>();

builder.Services.AddDbContext<CoffeeContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("CoffeeConnectionstring")));

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
