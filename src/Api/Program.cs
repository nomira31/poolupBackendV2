using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MediatR;
using System.Reflection;
using Poolup.Core.Interfaces;
using Poolup.Infrastructure.Persistence;
using Poolup.Application.Commands;

var builder = WebApplication.CreateBuilder(args);

// --- Load configuration ---
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// --- Database connection setup ---
var connectionString = builder.Configuration.GetConnectionString("AivenConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new Exception("Connection string 'AivenConnection' is missing from configuration.");
}

// Get password from EC2 environment and replace placeholder
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
if (!string.IsNullOrEmpty(dbPassword))
{
    connectionString = connectionString.Replace("${DB_PASSWORD}", dbPassword);
}

// Now register DbContext with the REPLACED connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(5);
    });
});

// --- Repositories ---
builder.Services.AddScoped<IWaitlistRepository, WaitlistRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// --- MediatR ---
// Note: Ensure you removed the 'MediatR.Extensions.Microsoft.DependencyInjection' package as discussed
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    cfg.RegisterServicesFromAssembly(typeof(AddWaitlistEntryCommand).Assembly);
});

// --- Identity password hasher ---
builder.Services.AddSingleton<IPasswordHasher<Poolup.Core.Entities.Authentication.User>, PasswordHasher<Poolup.Core.Entities.Authentication.User>>();

// --- Controllers / Swagger ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Auth / Authorization ---
builder.Services.AddAuthorization();

var app = builder.Build();

// --- Middleware ---

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();