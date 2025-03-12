using Gateways;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Updater;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddControllers();

// Load configuration
string environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Register services
builder.Services.Configure<UpdaterConfiguration>(builder.Configuration.GetSection("Application"));

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<UpdaterConfiguration>>().Value);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<UpdaterConfiguration>>().Value.HomeAssistant);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<UpdaterConfiguration>>().Value.Spoolman);

builder.Services.AddScoped<HomeAssistantClient>();
builder.Services.AddScoped<SpoolmanClient>();

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
