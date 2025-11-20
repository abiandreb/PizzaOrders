using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services;

public class ProductService(AppDbContext dbContext, ILogger<ProductService> logger) : IProductService
{
    public async Task<IList<ProductResponse>> GetAllProductsByType(int productType = 0, CancellationToken cancellationToken = default)
    {
        var products = await dbContext.Products
            .AsNoTracking()
            .Select(x => new ProductResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                BasePrice = x.BasePrice,
                HasToppings = x.HasToppings,
                ProductType = x.ProductType,
                ImageUrl = x.ImageUrl,
                Properties = x.ProductProperties
            })
            .Where(x => x.ProductType == (ProductType)productType) 
            .ToListAsync(cancellationToken);

        if (products.Count == 0)
        {
            logger.LogInformation("No products found");
        }
        
        return products;
    }

    public async Task<ProductResponse?> GetProductById(int id, CancellationToken cancellationToken = default)
    {
        var product = await dbContext.Products.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (product is null)
        {
            throw new InvalidOperationException("Product not found");
        }

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            BasePrice = product.BasePrice,
            HasToppings = product.HasToppings,
            ProductType = product.ProductType,
            ImageUrl = product.ImageUrl,
            Properties = product.ProductProperties
        };
    }
}

