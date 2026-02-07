using PizzaOrders.API.Hubs;
using PizzaOrders.API.Handlers;
using PizzaOrders.Application.Extensions;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Infrastructure.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add Aspire service defaults (OpenTelemetry, health checks, service discovery)
builder.AddServiceDefaults();

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddOpenApi();

// Add database and Identity
builder.Services.AddAppContext(builder.Configuration);

// Add application services
builder.Services.AddApplicationServices();

// Add Redis cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis")
        ?? throw new InvalidOperationException("Redis connection string not found.");
    options.InstanceName = "PizzaOrdersRedis";
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:3000", "http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Add JWT authentication
builder.Services.AddIdentityServices(builder.Configuration);

// Add SignalR
builder.Services.AddSignalR();
builder.Services.AddScoped<IOrderNotificationService, SignalROrderNotificationService>();

var app = builder.Build();

// Map Aspire default health check endpoints
app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowFrontend");

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<OrderHub>("/hubs/orders");

// Seed the database in development
if (app.Environment.IsDevelopment())
{
    await app.SeedDatabaseAsync();
}

app.Run();

// Expose Program class for integration testing
public partial class Program { }