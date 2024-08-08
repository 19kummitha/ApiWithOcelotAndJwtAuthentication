using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IProductRepository,ProductRepository>();
builder.Services.AddDbContext<ProductDbContext>(options =>
{
    var Connectionstring = builder.Configuration.GetConnectionString("productDbConnection");
    options.UseSqlServer(Connectionstring);
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
