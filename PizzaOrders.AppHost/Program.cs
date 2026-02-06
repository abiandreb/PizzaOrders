var builder = DistributedApplication.CreateBuilder(args);

// SQL Server Database
var sqlServer = builder.AddSqlServer("sqlserver")
    .WithDataVolume("pizzaorders-sqlserver-data")
    .WithLifetime(ContainerLifetime.Persistent);

var database = sqlServer.AddDatabase("PizzaOrders");

// Redis Cache
var redis = builder.AddRedis("redis")
    .WithDataVolume("pizzaorders-redis-data")
    .WithLifetime(ContainerLifetime.Persistent);

// Azure Blob Storage (Azurite emulator)
var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator(emulator => emulator
        .WithDataVolume("pizzaorders-azurite-data")
        .WithLifetime(ContainerLifetime.Persistent));

var blobs = storage.AddBlobs("blobs");

// PizzaOrders API
var api = builder.AddProject("api", "../PizzaOrders.API/PizzaOrders.API.csproj")
    .WithReference(database)
    .WaitFor(database)
    .WithReference(redis)
    .WaitFor(redis)
    .WithReference(blobs)
    .WithEnvironment("JWT__Secret", "PizzaOrdersSecretKeyForJWTAuthenticationMustBeAtLeast32BytesLong!")
    .WithEnvironment("JWT__Issuer", "https://localhost:7258")
    .WithEnvironment("JWT__Audience", "PizzaOrdersApp")
    .WithExternalHttpEndpoints();

// Frontend (optional - can be enabled when needed)
// Uncomment to include frontend in Aspire orchestration
// builder.AddNpmApp("frontend", "../frontend")
//     .WithReference(api)
//     .WithHttpEndpoint(port: 3000, env: "PORT")
//     .WithExternalHttpEndpoints();

builder.Build().Run();
