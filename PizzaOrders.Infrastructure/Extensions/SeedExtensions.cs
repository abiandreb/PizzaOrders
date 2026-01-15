using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzaOrders.Domain;
using PizzaOrders.Domain.Entities.AuthEntities;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Domain.Entities.Toppings; 
using PizzaOrders.Domain.Entities.Orders;
using PizzaOrders.Domain.Entities.Payment;

namespace PizzaOrders.Infrastructure.Extensions;

public static class SeedExtensions
{
    public static void SeedDomainData(this ModelBuilder modelBuilder)
    {
        var adminRole = new RoleEntity { Id = 1, Name = "Admin", NormalizedName = "ADMIN" };
        var userRole = new RoleEntity { Id = 2, Name = "User", NormalizedName = "USER" };

        modelBuilder.Entity<RoleEntity>().HasData(adminRole, userRole);

        // Admin User - Username: admin@pizzaorders.com, Password: Admin123!
        var adminUser = new UserEntity
        {
            Id = 1,
            UserName = "admin@pizzaorders.com",
            NormalizedUserName = "ADMIN@PIZZAORDERS.COM",
            Email = "admin@pizzaorders.com",
            NormalizedEmail = "ADMIN@PIZZAORDERS.COM",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAEAACcQAAAAECM8urhnYbjZrzD1hm7LpGKIzTlpRCtRn3xjyDrluo3SrVMaSio9XINiO2ZHm9kV6Q==",
            SecurityStamp = Guid.NewGuid().ToString()
        };

        // Regular User - Username: user@pizzaorders.com, Password: User123!
        var regularUser = new UserEntity
        {
            Id = 2,
            UserName = "user@pizzaorders.com",
            NormalizedUserName = "USER@PIZZAORDERS.COM",
            Email = "user@pizzaorders.com",
            NormalizedEmail = "USER@PIZZAORDERS.COM",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAEAACcQAAAAEIZDa3uUwJeh7hemK0lcE9Q7dgcZnGXUPuTAs1wvSIWwaAi4v6AW82fFkRlWHwzrGQ==",
            SecurityStamp = Guid.NewGuid().ToString()
        };

        // Test User - Username: test@pizzaorders.com, Password: Test123!
        var testUser = new UserEntity
        {
            Id = 3,
            UserName = "test@pizzaorders.com",
            NormalizedUserName = "TEST@PIZZAORDERS.COM",
            Email = "test@pizzaorders.com",
            NormalizedEmail = "TEST@PIZZAORDERS.COM",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAEAACcQAAAAEPGiCJ2u8hbbqfojZeJjZJBKSBtai0edJrx1y+zegh6wN6+YSWt/Q6F2Jjn22KkZgQ==",
            SecurityStamp = Guid.NewGuid().ToString()
        };

        modelBuilder.Entity<UserEntity>().HasData(adminUser, regularUser, testUser);

        modelBuilder.Entity<UserRoleEntity>().HasData(
            new UserRoleEntity { UserId = 1, RoleId = 1 },  // Admin is Admin role
            new UserRoleEntity { UserId = 2, RoleId = 2 },  // Regular User is User role
            new UserRoleEntity { UserId = 3, RoleId = 2 }   // Test User is User role
        );
        
        // Toppings
        modelBuilder.Entity<ToppingEntity>().HasData(
            new ToppingEntity { Id = 1, Name = "Pepperoni", Description = "Spicy sausage slices", Stock = 100, Price = 1.50m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new ToppingEntity { Id = 2, Name = "Mushrooms", Description = "Fresh sliced mushrooms", Stock = 100, Price = 1.00m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new ToppingEntity { Id = 3, Name = "Onions", Description = "Diced red onions", Stock = 100, Price = 0.75m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new ToppingEntity { Id = 4, Name = "Green Peppers", Description = "Sliced green bell peppers", Stock = 100, Price = 0.75m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new ToppingEntity { Id = 5, Name = "Black Olives", Description = "Pitted black olives", Stock = 100, Price = 1.00m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new ToppingEntity { Id = 6, Name = "Extra Cheese", Description = "Additional mozzarella cheese", Stock = 100, Price = 1.25m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new ToppingEntity { Id = 7, Name = "Ham", Description = "Smoked ham", Stock = 100, Price = 1.50m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new ToppingEntity { Id = 8, Name = "Pineapple", Description = "Sweet pineapple chunks", Stock = 100, Price = 1.00m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new ToppingEntity { Id = 9, Name = "BBQ Chicken", Description = "Grilled BBQ chicken pieces", Stock = 100, Price = 2.00m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new ToppingEntity { Id = 10, Name = "Buffalo Chicken", Description = "Spicy buffalo chicken pieces", Stock = 100, Price = 2.00m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        );

        modelBuilder.Entity<ProductEntity>().HasData(
            // Pizzas
            new ProductEntity
            {
                Id = 1, Name = "Margherita", Description = "Classic cheese and tomato pizza", BasePrice = 8.99m,
                HasToppings = true, ImageUrl = "https://example.com/images/margherita.jpg",
                ProductType = ProductType.Pizza, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                ProductProperties = new ProductProperties
                {
                    SizeOptions = new List<SizeOption>
                    {
                        new SizeOption { Size = "Small", Price = 0.00m },
                        new SizeOption { Size = "Large", Price = 3.00m }
                    },
                    DefaultToppingIds = new List<int> { 6 }, // Extra Cheese
                    AvailableExtraToppingIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                }
            },
            new ProductEntity
            {
                Id = 2, Name = "Pepperoni", Description = "Pizza with pepperoni slices", BasePrice = 10.99m,
                HasToppings = true, ImageUrl = "https://example.com/images/pepperoni.jpg",
                ProductType = ProductType.Pizza, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                ProductProperties = new ProductProperties
                {
                    SizeOptions = new List<SizeOption>
                    {
                        new SizeOption { Size = "Small", Price = 0.00m },
                        new SizeOption { Size = "Large", Price = 3.00m }
                    },
                    DefaultToppingIds = new List<int> { 1 }, // Pepperoni
                    AvailableExtraToppingIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                }
            },
            new ProductEntity
            {
                Id = 3, Name = "Hawaiian", Description = "Pizza with ham and pineapple", BasePrice = 11.99m,
                HasToppings = true, ImageUrl = "https://example.com/images/hawaiian.jpg",
                ProductType = ProductType.Pizza, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                ProductProperties = new ProductProperties
                {
                    SizeOptions = new List<SizeOption>
                    {
                        new SizeOption { Size = "Small", Price = 0.00m },
                        new SizeOption { Size = "Large", Price = 3.00m }
                    },
                    DefaultToppingIds = new List<int> { 7, 8 }, // Ham, Pineapple
                    AvailableExtraToppingIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                }
            },
            new ProductEntity
            {
                Id = 4, Name = "BBQ Chicken", Description = "Pizza with BBQ chicken and onions", BasePrice = 12.99m,
                HasToppings = true, ImageUrl = "https://example.com/images/bbq-chicken.jpg",
                ProductType = ProductType.Pizza, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                ProductProperties = new ProductProperties
                {
                    SizeOptions = new List<SizeOption>
                    {
                        new SizeOption { Size = "Small", Price = 0.00m },
                        new SizeOption { Size = "Large", Price = 3.00m }
                    },
                    DefaultToppingIds = new List<int> { 3, 9 }, // Onions, BBQ Chicken
                    AvailableExtraToppingIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                }
            },
            new ProductEntity
            {
                Id = 5, Name = "Veggie", Description = "Pizza with mixed vegetables", BasePrice = 9.99m,
                HasToppings = true, ImageUrl = "https://example.com/images/veggie.jpg",
                ProductType = ProductType.Pizza, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                ProductProperties = new ProductProperties
                {
                    SizeOptions = new List<SizeOption>
                    {
                        new SizeOption { Size = "Small", Price = 0.00m },
                        new SizeOption { Size = "Large", Price = 3.00m }
                    },
                    DefaultToppingIds = new List<int> { 2, 3, 4, 5 }, // Mushrooms, Onions, Green Peppers, Black Olives
                    AvailableExtraToppingIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                }
            },
            new ProductEntity
            {
                Id = 6, Name = "Meat Lovers", Description = "Pizza with a variety of meats", BasePrice = 13.99m,
                HasToppings = true, ImageUrl = "https://example.com/images/meat-lovers.jpg",
                ProductType = ProductType.Pizza, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                ProductProperties = new ProductProperties
                {
                    SizeOptions = new List<SizeOption>
                    {
                        new SizeOption { Size = "Small", Price = 0.00m },
                        new SizeOption { Size = "Large", Price = 3.00m }
                    },
                    DefaultToppingIds = new List<int> { 1, 7, 9 }, // Pepperoni, Ham, BBQ Chicken
                    AvailableExtraToppingIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                }
            },
            new ProductEntity
            {
                Id = 7, Name = "Buffalo Chicken", Description = "Spicy buffalo chicken pizza", BasePrice = 12.99m,
                HasToppings = true, ImageUrl = "https://example.com/images/buffalo-chicken.jpg",
                ProductType = ProductType.Pizza, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                ProductProperties = new ProductProperties
                {
                    SizeOptions = new List<SizeOption>
                    {
                        new SizeOption { Size = "Small", Price = 0.00m },
                        new SizeOption { Size = "Large", Price = 3.00m }
                    },
                    DefaultToppingIds = new List<int> { 10 }, // Buffalo Chicken
                    AvailableExtraToppingIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                }
            },
            new ProductEntity
            {
                Id = 8, Name = "Supreme", Description = "The ultimate pizza with everything", BasePrice = 14.99m,
                HasToppings = true, ImageUrl = "https://example.com/images/supreme.jpg",
                ProductType = ProductType.Pizza, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                ProductProperties = new ProductProperties
                {
                    SizeOptions = new List<SizeOption>
                    {
                        new SizeOption { Size = "Small", Price = 0.00m },
                        new SizeOption { Size = "Large", Price = 3.00m }
                    },
                    DefaultToppingIds = new List<int> { 1, 2, 3, 4, 5, 6, 7 }, // Pepperoni, Mushrooms, Onions, Green Peppers, Black Olives, Extra Cheese, Ham
                    AvailableExtraToppingIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                }
            },
            new ProductEntity
            {
                Id = 9, Name = "Four Cheese", Description = "Pizza with four types of cheese", BasePrice = 11.49m,
                HasToppings = true, ImageUrl = "https://example.com/images/four-cheese.jpg",
                ProductType = ProductType.Pizza, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                ProductProperties = new ProductProperties
                {
                    SizeOptions = new List<SizeOption>
                    {
                        new SizeOption { Size = "Small", Price = 0.00m },
                        new SizeOption { Size = "Large", Price = 3.00m }
                    },
                    DefaultToppingIds = new List<int> { 6 }, // Extra Cheese
                    AvailableExtraToppingIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                }
            },
            new ProductEntity
            {
                Id = 10, Name = "Mushroom", Description = "Pizza with fresh mushrooms", BasePrice = 10.49m,
                HasToppings = true, ImageUrl = "https://example.com/images/mushroom.jpg",
                ProductType = ProductType.Pizza, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                ProductProperties = new ProductProperties
                {
                    SizeOptions = new List<SizeOption>
                    {
                        new SizeOption { Size = "Small", Price = 0.00m },
                        new SizeOption { Size = "Large", Price = 3.00m }
                    },
                    DefaultToppingIds = new List<int> { 2 }, // Mushrooms
                    AvailableExtraToppingIds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }
                }
            },
            // Starters
            new ProductEntity
            {
                Id = 11, Name = "French Fries", Description = "Crispy golden french fries", BasePrice = 3.99m,
                HasToppings = false, ImageUrl = "https://example.com/images/french-fries.jpg",
                ProductType = ProductType.Starter, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            },
            // Drinks
            new ProductEntity
            {
                Id = 12, Name = "Coke", Description = "Classic Coca-Cola", BasePrice = 1.99m, HasToppings = false,
                ImageUrl = "https://example.com/images/coke.jpg", ProductType = ProductType.Drink, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            },
            // Drinks
            new ProductEntity
            {
                Id = 13, Name = "Sprite", Description = "Lemon-lime flavored soft drink", BasePrice = 1.99m,
                HasToppings = false, ImageUrl = "https://example.com/images/sprite.jpg",
                ProductType = ProductType.Drink, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow
            }
        );

        // Orders
        modelBuilder.Entity<OrderEntity>().HasData(
            new OrderEntity { Id = 1, UserId = 2, TotalPrice = 12.49m, Status = OrderStatus.Completed, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new OrderEntity { Id = 2, UserId = 2, TotalPrice = 15.97m, Status = OrderStatus.Completed, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        );

        // Order Items
        modelBuilder.Entity<OrderItemEntity>().HasData(
            // Order 1 Items
            new OrderItemEntity
            {
                Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, ItemPrice = 8.99m, TotalPrice = 12.49m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                ItemModifiers = new ItemModifiers
                {
                    Size = "Small",
                    ExtraToppings = new List<SelectedItemTopping>
                    {
                        new SelectedItemTopping { ToppingId = 1, Quantity = 1 }, // Extra Pepperoni
                        new SelectedItemTopping { ToppingId = 2, Quantity = 1 } // Extra Mushrooms
                    }
                }
            },
            new OrderItemEntity { Id = 2, OrderId = 1, ProductId = 12, Quantity = 1, ItemPrice = 1.99m, TotalPrice = 1.99m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },

            // Order 2 Items
            new OrderItemEntity
            {
                Id = 3, OrderId = 2, ProductId = 2, Quantity = 1, ItemPrice = 10.99m, TotalPrice = 13.99m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow,
                ItemModifiers = new ItemModifiers
                {
                    Size = "Large",
                    ExtraToppings = new List<SelectedItemTopping>()
                }
            },
            new OrderItemEntity { Id = 4, OrderId = 2, ProductId = 11, Quantity = 1, ItemPrice = 3.99m, TotalPrice = 3.99m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new OrderItemEntity { Id = 5, OrderId = 2, ProductId = 13, Quantity = 1, ItemPrice = 1.99m, TotalPrice = 1.99m, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        );

        // Payments
        modelBuilder.Entity<PaymentEntity>().HasData(
            new PaymentEntity { Id = 1, OrderId = 1, Method = PaymentMethod.Online, Status = PaymentStatus.Paid, Amount = 12.49m, TransactionId = "TRN001", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, ConfirmedAt = DateTime.UtcNow, Gateway = "Stripe" },
            new PaymentEntity { Id = 2, OrderId = 2, Method = PaymentMethod.Cash, Status = PaymentStatus.Pending, Amount = 15.97m, TransactionId = "TRN002", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, ConfirmedAt = null, Gateway = "Cash" }
        );
    }
}