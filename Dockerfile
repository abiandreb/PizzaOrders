# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY PizzaOrders.sln ./
COPY PizzaOrders.Domain/PizzaOrders.Domain.csproj PizzaOrders.Domain/
COPY PizzaOrders.Application/PizzaOrders.Application.csproj PizzaOrders.Application/
COPY PizzaOrders.Infrastructure/PizzaOrders.Infrastructure.csproj PizzaOrders.Infrastructure/
COPY PizzaOrders.API/PizzaOrders.API.csproj PizzaOrders.API/

# Restore dependencies
RUN dotnet restore PizzaOrders.API/PizzaOrders.API.csproj

# Copy all source code
COPY PizzaOrders.Domain/ PizzaOrders.Domain/
COPY PizzaOrders.Application/ PizzaOrders.Application/
COPY PizzaOrders.Infrastructure/ PizzaOrders.Infrastructure/
COPY PizzaOrders.API/ PizzaOrders.API/

# Build and publish
WORKDIR /src/PizzaOrders.API
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser

COPY --from=build /app/publish .

# Change ownership to non-root user
RUN chown -R appuser:appuser /app
USER appuser

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "PizzaOrders.API.dll"]
