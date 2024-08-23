using AuthenticationWebApi.Data;
using AuthenticationWebApi.ExtensionHandling;
using AuthenticationWebApi.TokenManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddDbContext<AuthenticationDbContext>(options =>
{
    var Connectionstring = builder.Configuration.GetConnectionString("AuthDbConnection");
    options.UseSqlServer(Connectionstring);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthenticationDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddControllers();

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.ConfigureExceptionHandler(logger);

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
