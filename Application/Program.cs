using Domain;
using Gateways;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = builder.Configuration.GetSection("Application").Get<UpdaterConfiguration>();

builder.Services
    .AddDomain()
    .AddGateways(configuration);

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});

var app = builder.Build();
    
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    using var scope = app.Services.CreateScope();
    var gatewayChecker = scope.ServiceProvider.GetRequiredService<GatewayChecker>();

    await gatewayChecker.CheckGatewayConnectionAsync();
});

app.Run();
