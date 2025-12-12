using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PizzaOrders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasToppings = table.Column<bool>(type: "bit", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    ProductProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Toppings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Toppings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsGuest = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateRevoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ItemPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemModifiers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Method = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gateway = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BasePrice", "CreatedAt", "Description", "HasToppings", "ImageUrl", "Name", "ProductProperties", "ProductType", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 8.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(1814), "Classic cheese and tomato pizza", true, "https://example.com/images/margherita.jpg", "Margherita", "{\"SizeOptions\":[{\"Size\":\"Small\",\"Price\":0.00},{\"Size\":\"Large\",\"Price\":3.00}],\"DefaultToppingIds\":[6],\"AvailableExtraToppingIds\":[1,2,3,4,5,6,7,8,9,10]}", 0, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(1815) },
                    { 2, 10.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3776), "Pizza with pepperoni slices", true, "https://example.com/images/pepperoni.jpg", "Pepperoni", "{\"SizeOptions\":[{\"Size\":\"Small\",\"Price\":0.00},{\"Size\":\"Large\",\"Price\":3.00}],\"DefaultToppingIds\":[1],\"AvailableExtraToppingIds\":[1,2,3,4,5,6,7,8,9,10]}", 0, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3776) },
                    { 3, 11.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3788), "Pizza with ham and pineapple", true, "https://example.com/images/hawaiian.jpg", "Hawaiian", "{\"SizeOptions\":[{\"Size\":\"Small\",\"Price\":0.00},{\"Size\":\"Large\",\"Price\":3.00}],\"DefaultToppingIds\":[7,8],\"AvailableExtraToppingIds\":[1,2,3,4,5,6,7,8,9,10]}", 0, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3788) },
                    { 4, 12.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3794), "Pizza with BBQ chicken and onions", true, "https://example.com/images/bbq-chicken.jpg", "BBQ Chicken", "{\"SizeOptions\":[{\"Size\":\"Small\",\"Price\":0.00},{\"Size\":\"Large\",\"Price\":3.00}],\"DefaultToppingIds\":[3,9],\"AvailableExtraToppingIds\":[1,2,3,4,5,6,7,8,9,10]}", 0, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3794) },
                    { 5, 9.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3866), "Pizza with mixed vegetables", true, "https://example.com/images/veggie.jpg", "Veggie", "{\"SizeOptions\":[{\"Size\":\"Small\",\"Price\":0.00},{\"Size\":\"Large\",\"Price\":3.00}],\"DefaultToppingIds\":[2,3,4,5],\"AvailableExtraToppingIds\":[1,2,3,4,5,6,7,8,9,10]}", 0, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3867) },
                    { 6, 13.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3873), "Pizza with a variety of meats", true, "https://example.com/images/meat-lovers.jpg", "Meat Lovers", "{\"SizeOptions\":[{\"Size\":\"Small\",\"Price\":0.00},{\"Size\":\"Large\",\"Price\":3.00}],\"DefaultToppingIds\":[1,7,9],\"AvailableExtraToppingIds\":[1,2,3,4,5,6,7,8,9,10]}", 0, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3873) },
                    { 7, 12.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3878), "Spicy buffalo chicken pizza", true, "https://example.com/images/buffalo-chicken.jpg", "Buffalo Chicken", "{\"SizeOptions\":[{\"Size\":\"Small\",\"Price\":0.00},{\"Size\":\"Large\",\"Price\":3.00}],\"DefaultToppingIds\":[10],\"AvailableExtraToppingIds\":[1,2,3,4,5,6,7,8,9,10]}", 0, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3879) },
                    { 8, 14.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3894), "The ultimate pizza with everything", true, "https://example.com/images/supreme.jpg", "Supreme", "{\"SizeOptions\":[{\"Size\":\"Small\",\"Price\":0.00},{\"Size\":\"Large\",\"Price\":3.00}],\"DefaultToppingIds\":[1,2,3,4,5,6,7],\"AvailableExtraToppingIds\":[1,2,3,4,5,6,7,8,9,10]}", 0, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3894) },
                    { 9, 11.49m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3900), "Pizza with four types of cheese", true, "https://example.com/images/four-cheese.jpg", "Four Cheese", "{\"SizeOptions\":[{\"Size\":\"Small\",\"Price\":0.00},{\"Size\":\"Large\",\"Price\":3.00}],\"DefaultToppingIds\":[6],\"AvailableExtraToppingIds\":[1,2,3,4,5,6,7,8,9,10]}", 0, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3900) },
                    { 10, 10.49m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3905), "Pizza with fresh mushrooms", true, "https://example.com/images/mushroom.jpg", "Mushroom", "{\"SizeOptions\":[{\"Size\":\"Small\",\"Price\":0.00},{\"Size\":\"Large\",\"Price\":3.00}],\"DefaultToppingIds\":[2],\"AvailableExtraToppingIds\":[1,2,3,4,5,6,7,8,9,10]}", 0, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3905) },
                    { 11, 3.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3918), "Crispy golden french fries", false, "https://example.com/images/french-fries.jpg", "French Fries", null, 2, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3918) },
                    { 12, 1.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3920), "Classic Coca-Cola", false, "https://example.com/images/coke.jpg", "Coke", null, 1, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3920) },
                    { 13, 1.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3921), "Lemon-lime flavored soft drink", false, "https://example.com/images/sprite.jpg", "Sprite", null, 1, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3922) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "5a117e0a-f074-4954-a915-e81ede6bfaa9", "Admin", "ADMIN" },
                    { 2, "1e5f5fa0-cf31-4861-bcfc-118962586af9", "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "Toppings",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "Price", "Stock", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 17, 16, 44, 14, 214, DateTimeKind.Utc).AddTicks(9780), "Spicy sausage slices", "Pepperoni", 1.50m, 100, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(11) },
                    { 2, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(207), "Fresh sliced mushrooms", "Mushrooms", 1.00m, 100, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(208) },
                    { 3, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(209), "Diced red onions", "Onions", 0.75m, 100, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(210) },
                    { 4, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(211), "Sliced green bell peppers", "Green Peppers", 0.75m, 100, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(212) },
                    { 5, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(213), "Pitted black olives", "Black Olives", 1.00m, 100, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(213) },
                    { 6, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(214), "Additional mozzarella cheese", "Extra Cheese", 1.25m, 100, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(214) },
                    { 7, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(215), "Smoked ham", "Ham", 1.50m, 100, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(216) },
                    { 8, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(217), "Sweet pineapple chunks", "Pineapple", 1.00m, 100, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(217) },
                    { 9, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(218), "Grilled BBQ chicken pieces", "BBQ Chicken", 2.00m, 100, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(218) },
                    { 10, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(219), "Spicy buffalo chicken pieces", "Buffalo Chicken", 2.00m, 100, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(220) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsGuest", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { 1, 0, null, "ef4576ca-01dd-42a9-a8dd-2200852bcba0", "admin@example.com", true, false, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEJ/n/y4nL5z0mJ6/BEiXzY3T2gR8gUaZa/tG2gR8gUaZa/tG2gR8gUaZa/tG2g==", null, false, "9239922b-2875-4350-a926-2184462719a4", false, "admin" },
                    { 2, 0, null, "a267eb5d-5e8e-408a-8c29-f8d0d6cbc080", "user@example.com", true, false, false, null, "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAEJ/n/y4nL5z0mJ6/BEiXzY3T2gR8gUaZa/tG2gR8gUaZa/tG2gR8gUaZa/tG2g==", null, false, "e178d197-36e7-4935-a744-83a3162b77a7", false, "user" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CreatedAt", "PaymentId", "Status", "TotalPrice", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(5395), null, 8, 12.49m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(5396), 2 },
                    { 2, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(5400), null, 8, 15.97m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(5400), 2 }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "Id" },
                values: new object[,]
                {
                    { 1, 1, 0 },
                    { 2, 2, 0 }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "CreatedAt", "ItemModifiers", "ItemPrice", "OrderId", "ProductId", "Quantity", "TotalPrice", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(6718), "{\"Size\":\"Small\",\"ExtraToppings\":[{\"ToppingId\":1,\"Quantity\":1},{\"ToppingId\":2,\"Quantity\":1}]}", 8.99m, 1, 1, 1, 12.49m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(6718) },
                    { 2, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8261), null, 1.99m, 1, 12, 1, 1.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8262) },
                    { 3, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8263), "{\"Size\":\"Large\",\"ExtraToppings\":[]}", 10.99m, 2, 2, 1, 13.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8264) },
                    { 4, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8267), null, 3.99m, 2, 11, 1, 3.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8268) },
                    { 5, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8269), null, 1.99m, 2, 13, 1, 1.99m, new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8269) }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "Amount", "ConfirmedAt", "CreatedAt", "Gateway", "Method", "OrderId", "Status", "TransactionId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 12.49m, new DateTime(2025, 11, 17, 16, 44, 14, 216, DateTimeKind.Utc).AddTicks(106), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(9887), "Stripe", 1, 1, 1, "TRN001", new DateTime(2025, 11, 17, 16, 44, 14, 216, DateTimeKind.Utc).AddTicks(105) },
                    { 2, 15.97m, null, new DateTime(2025, 11, 17, 16, 44, 14, 216, DateTimeKind.Utc).AddTicks(585), "Cash", 0, 2, 0, "TRN002", new DateTime(2025, 11, 17, 16, 44, 14, 216, DateTimeKind.Utc).AddTicks(586) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "Toppings");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
