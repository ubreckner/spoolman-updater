using Gateways;
using Updater;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration.GetSection("Application").Get<UpdaterConfiguration>();

// 🔹 Register the configuration instance as a singleton
builder.Services.AddSingleton(configuration.Spoolman);
builder.Services.AddSingleton(configuration.HomeAssistant);
builder.Services.AddScoped<HomeAssistantClient>();
builder.Services.AddScoped<SpoolmanClient>();

var app = builder.Build();
    
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
