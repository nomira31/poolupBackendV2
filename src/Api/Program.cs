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

// --- Database connection ---
var aivenConn = builder.Configuration.GetConnectionString("AivenConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(aivenConn, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(5);
    });
});

// --- Repositories ---
builder.Services.AddScoped<IWaitlistRepository, WaitlistRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// --- MediatR ---
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());       // API commands/queries
    cfg.RegisterServicesFromAssembly(typeof(AddWaitlistEntryCommand).Assembly); // Application layer
});

// --- Optional: Identity password hasher ---
builder.Services.AddSingleton<IPasswordHasher<Poolup.Core.Entities.Authentication.User>, PasswordHasher<Poolup.Core.Entities.Authentication.User>>();

// --- Controllers / Swagger ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- Auth / Authorization ---
builder.Services.AddAuthorization();

// --- Build app ---
var app = builder.Build();

// --- Middleware ---
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

// If you have authentication later
app.UseAuthentication();
app.UseAuthorization();

// --- Map controllers ---
app.MapControllers();

// --- Run ---
app.Run();
