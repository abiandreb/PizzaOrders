var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.PizzaOrders_API>("pizzaorders-api");

builder.AddProject<Projects.PizzaOrders_Tests>("pizzaorders-tests");

builder.Build().Run();
