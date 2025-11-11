using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PizzaOrders.Domain.Entities.AuthEntities;
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Domain.Entities.Payment;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Domain.Entities.Toppings;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.Infrastructure.Extensions;

public static class SeedExtensions
{
    public static void SeedDomainData(this ModelBuilder modelBuilder)
    {
        var productToppings = new List<ProductToppingEntity>
        {
            // Margherita
            new() { ProductId = 1, ToppingId = 1 },
    
            // Pepperoni
            new() { ProductId = 2, ToppingId = 1 },
            new() { ProductId = 2, ToppingId = 2 },
    
            // Hawaiian
            new() { ProductId = 3, ToppingId = 1 },
            new() { ProductId = 3, ToppingId = 6 },
            new() { ProductId = 3, ToppingId = 7 },
    
            // Four Cheese
            new() { ProductId = 4, ToppingId = 1 },
            new() { ProductId = 4, ToppingId = 10 },
    
            // BBQ Chicken
            new() { ProductId = 5, ToppingId = 9 },
            new() { ProductId = 5, ToppingId = 6 },
    
            // Vegetarian
            new() { ProductId = 6, ToppingId = 3 },
            new() { ProductId = 6, ToppingId = 4 },
            new() { ProductId = 6, ToppingId = 5 },
            new() { ProductId = 6, ToppingId = 8 },
    
            // Meat Lover
            new() { ProductId = 7, ToppingId = 2 },
            new() { ProductId = 7, ToppingId = 6 },
            new() { ProductId = 7, ToppingId = 9 },
    
            // Capricciosa
            new() { ProductId = 8, ToppingId = 6 },
            new() { ProductId = 8, ToppingId = 3 },
            new() { ProductId = 8, ToppingId = 8 },
        };

        var products = new List<ProductEntity>
        {
            new() { Id = 1, Name = "Margherita", Description = "Classic tomato sauce & mozzarella", ProductType = ProductType.Pizza, BasePrice = 25.0m, HasToppings = true, ImageUrl = "/img/pizza/margherita.jpg" },
            new() { Id = 2, Name = "Pepperoni", Description = "Spicy pepperoni & cheese", ProductType = ProductType.Pizza, BasePrice = 28.0m, HasToppings = true, ImageUrl = "/img/pizza/pepperoni.jpg" },
            new() { Id = 3, Name = "Hawaiian", Description = "Ham, pineapple & mozzarella", ProductType = ProductType.Pizza, BasePrice = 30.0m, HasToppings = true, ImageUrl = "/img/pizza/hawaiian.jpg" },
            new() { Id = 4, Name = "Four Cheese", Description = "Mozzarella, cheddar, blue cheese, parmesan", ProductType = ProductType.Pizza, BasePrice = 32.0m, HasToppings = true, ImageUrl = "/img/pizza/fourcheese.jpg" },
            new() { Id = 5, Name = "BBQ Chicken", Description = "Chicken, bacon, BBQ sauce", ProductType = ProductType.Pizza, BasePrice = 33.0m, HasToppings = true, ImageUrl = "/img/pizza/bbqchicken.jpg" },
            new() { Id = 6, Name = "Vegetarian", Description = "Mushrooms, bell peppers, onion, olives", ProductType = ProductType.Pizza, BasePrice = 27.0m, HasToppings = true, ImageUrl = "/img/pizza/vegetarian.jpg" },
            new() { Id = 7, Name = "Meat Lover", Description = "Ham, bacon, pepperoni, BBQ sauce", ProductType = ProductType.Pizza, BasePrice = 35.0m, HasToppings = true, ImageUrl = "/img/pizza/meatlovers.jpg" },
            new() { Id = 8, Name = "Capricciosa", Description = "Ham, mushrooms, olives, artichokes", ProductType = ProductType.Pizza, BasePrice = 31.0m, HasToppings = true, ImageUrl = "/img/pizza/capricciosa.jpg" },
            new() { Id = 9, Name = "Coke", Description = "0.5L Coca-Cola bottle", ProductType = ProductType.Drink, BasePrice = 6.0m, HasToppings = false, ImageUrl = "/img/drinks/coke.jpg" },
            new() { Id = 10, Name = "Pepsi", Description = "0.5L Pepsi bottle", ProductType = ProductType.Drink, BasePrice = 6.0m, HasToppings = false, ImageUrl = "/img/drinks/pepsi.jpg" },
            new() { Id = 11, Name = "7Up", Description = "0.5L 7Up bottle", ProductType = ProductType.Drink, BasePrice = 6.0m, HasToppings = false, ImageUrl = "/img/drinks/7up.jpg" },
            new() { Id = 12, Name = "Fries with Sauce", Description = "Crispy fries with garlic sauce", ProductType = ProductType.Starter, BasePrice = 10.0m, HasToppings = false, ImageUrl = "/img/starters/fries.jpg" },
            new() { Id = 13, Name = "Onion Rings", Description = "Deep-fried onion rings", ProductType = ProductType.Starter, BasePrice = 9.0m, HasToppings = false, ImageUrl = "/img/starters/onionrings.jpg" }
        };
        
        var toppings = new List<ToppingEntity>
        {
            new() { Id = 1, Name = "Mozzarella", Description = "Fresh mozzarella cheese", Price = 2.0m },
            new() { Id = 2, Name = "Pepperoni", Description = "Spicy pepperoni slices", Price = 2.5m },
            new() { Id = 3, Name = "Mushrooms", Description = "Fresh champignon mushrooms", Price = 1.5m },
            new() { Id = 4, Name = "Onion", Description = "Sliced red onion", Price = 1.0m },
            new() { Id = 5, Name = "Bell Pepper", Description = "Sweet bell peppers", Price = 1.2m },
            new() { Id = 6, Name = "Ham", Description = "Classic ham slices", Price = 2.5m },
            new() { Id = 7, Name = "Pineapple", Description = "Juicy pineapple chunks", Price = 1.8m },
            new() { Id = 8, Name = "Olives", Description = "Black olives", Price = 1.5m },
            new() { Id = 9, Name = "Bacon", Description = "Smoked crispy bacon", Price = 2.7m },
            new() { Id = 10, Name = "Extra Cheese", Description = "Double mozzarella layer", Price = 2.0m }
        };

        var order = new OrderEntity
        {
            Id = 1,
            Status = OrderStatus.Paid,
            TotalPrice = 39.2m,
            PaymentId = 1,
            UserId = 1
        };

        var payment = new PaymentEntity
        {
            Id = 1,
            OrderId = 1,
            Amount = 39.2m,
            Method = PaymentMethod.Online,
            Status = PaymentStatus.Paid,
            Gateway = "Stripe",
            TransactionId = "txn_1J23456789ABCDEFG",
        };
        
        var orderItems = new List<OrderItemEntity>
        {
            new()
            {
                Id = 1,
                OrderId = 1,
                ProductId = 2, // Pepperoni
                Quantity = 1,
                ItemPrice = 28.0m,
                TotalPrice = 33.2m
            },
            new()
            {
                Id = 2,
                OrderId = 1,
                ProductId = 9, // Coke
                Quantity = 1,
                ItemPrice = 6.0m,
                TotalPrice = 6.0m
            }
        };
        
        var orderItemToppings = new List<OrderItemToppingEntity>
        {
            new() { Id = 1, OrderItemId = 1, ToppingId = 9, Price = 2.7m, Quantity = 1},  // Bacon
            new() { Id = 2, OrderItemId = 1, ToppingId = 10, Price = 2.5m, Quantity = 1} // Extra Cheese
        };
        
        modelBuilder.Entity<OrderEntity>().HasData(order);
        modelBuilder.Entity<PaymentEntity>().HasData(payment);
        modelBuilder.Entity<OrderItemEntity>().HasData(orderItems);
        modelBuilder.Entity<OrderItemToppingEntity>().HasData(orderItemToppings);
        
        modelBuilder.Entity<ToppingEntity>().HasData(toppings);
        modelBuilder.Entity<ProductEntity>().HasData(products);
        modelBuilder.Entity<ProductToppingEntity>().HasData(productToppings);
    }
}