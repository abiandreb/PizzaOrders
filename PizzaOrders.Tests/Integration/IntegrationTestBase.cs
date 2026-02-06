using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PizzaOrders.Infrastructure.Data;
using Testcontainers.MsSql;
using Testcontainers.Redis;

namespace PizzaOrders.Tests.Integration;

/// <summary>
/// Base class for integration tests that provides containerized SQL Server and Redis.
/// Tests using this base will run against real infrastructure, not in-memory databases.
/// </summary>
[TestFixture]
public abstract class IntegrationTestBase
{
    private MsSqlContainer _sqlContainer = null!;
    private RedisContainer _redisContainer = null!;
    protected WebApplicationFactory<Program> Factory = null!;
    protected HttpClient Client = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Start SQL Server container
        _sqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .Build();

        await _sqlContainer.StartAsync();

        // Start Redis container
        _redisContainer = new RedisBuilder()
            .WithImage("redis:7-alpine")
            .Build();

        await _redisContainer.StartAsync();

        // Create WebApplicationFactory with containerized services
        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add DbContext using the test container connection string
                    services.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseSqlServer(_sqlContainer.GetConnectionString());
                    });

                    // Remove existing Redis cache registration and use test container
                    var redisDescriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(Microsoft.Extensions.Caching.Distributed.IDistributedCache));

                    if (redisDescriptor != null)
                    {
                        services.Remove(redisDescriptor);
                    }

                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = _redisContainer.GetConnectionString();
                        options.InstanceName = "TestRedis";
                    });
                });

                builder.UseEnvironment("Testing");
            });

        Client = Factory.CreateClient();

        // Ensure database is created and migrated
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        Client?.Dispose();
        Factory?.Dispose();

        if (_sqlContainer != null)
        {
            await _sqlContainer.DisposeAsync();
        }

        if (_redisContainer != null)
        {
            await _redisContainer.DisposeAsync();
        }
    }

    protected T GetService<T>() where T : notnull
    {
        using var scope = Factory.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }

    protected async Task ExecuteDbContextAsync(Func<AppDbContext, Task> action)
    {
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await action(dbContext);
    }
}
