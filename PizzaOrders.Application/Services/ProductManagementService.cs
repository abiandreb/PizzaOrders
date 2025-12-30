using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Application.Services
{
    public class ProductManagementService(AppDbContext context) : IProductManagementService
    {
        public async Task<IEnumerable<ProductResponse>> GetAllProductsAsync()
        {
            var products = await context.Products
                .OrderBy(p => p.Name)
                .Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    BasePrice = p.BasePrice,
                    HasToppings = p.HasToppings,
                    ProductType = p.ProductType,
                    ImageUrl = p.ImageUrl,
                    Properties = p.ProductProperties
                }).ToListAsync();
            
            return products;
        }
        
        public async Task<ProductResponse> CreateProductAsync(CreateProductRequestDto request)
        {
            var product = new ProductEntity
            {
                Name = request.Name,
                Description = request.Description,
                BasePrice = request.BasePrice,
                HasToppings = request.HasToppings,
                ProductType = request.ProductType,
                ImageUrl = request.ImageUrl,
                ProductProperties = request.Properties
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

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

        public async Task<ProductResponse> UpdateProductAsync(UpdateProductRequestDto request)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == request.Id);

            if (product == null)
            {
                throw new System.InvalidOperationException("Product not found.");
            }

            if (request.Name != null) product.Name = request.Name;
            if (request.Description != null) product.Description = request.Description;
            if (request.BasePrice.HasValue) product.BasePrice = request.BasePrice.Value;
            if (request.HasToppings.HasValue) product.HasToppings = request.HasToppings.Value;
            if (request.ProductType.HasValue) product.ProductType = request.ProductType.Value;
            if (request.ImageUrl != null) product.ImageUrl = request.ImageUrl;
            if (request.Properties != null) product.ProductProperties = request.Properties;

            await context.SaveChangesAsync();
            
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

        public async Task DeleteProductAsync(int productId)
        {
            var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                throw new System.InvalidOperationException("Product not found.");
            }

            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }
}
