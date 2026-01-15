using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaOrders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductImageSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductImage_FullUrl",
                schema: "domain",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductImage_MediumUrl",
                schema: "domain",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductImage_ThumbnailUrl",
                schema: "domain",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ItemModifiers", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(1983), "{\"Size\":\"Small\",\"ExtraToppings\":[{\"ToppingId\":1,\"Quantity\":1,\"Price\":null},{\"ToppingId\":2,\"Quantity\":1,\"Price\":null}]}", new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(1984) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(3578), new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(3579) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(3580), new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(3580) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(3584), new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(3584) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(3585), new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(3586) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(528), new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(529) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(534), new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(534) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(5223), new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(4999), new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(5222) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(5700), new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(5701) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(6728), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(6729) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8865), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8866) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8877), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8878) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8884), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8884) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8901), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8901) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8907), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8908) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8913), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8913) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8918), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8919) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8935), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8935) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8940), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8941) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8946), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8946) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8980), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8980) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8982), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(8982) });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "88287aff-1c62-4b39-9cdd-76eac277aedf");

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "5b2f0962-71d3-4552-b4ce-2a3220c83453");

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(4611), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(4832) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5033), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5033) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5034), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5035) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5036), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5036) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5037), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5038) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5092), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5093) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5094), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5094) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5095), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5095) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5097), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5097) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5098), new DateTime(2026, 1, 15, 14, 30, 28, 228, DateTimeKind.Utc).AddTicks(5098) });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "4468d8bd-8b88-4d2e-8555-18e54131bae6");

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "02df7502-4e63-4185-a9f2-482474f3809d");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductImage_FullUrl",
                schema: "domain",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductImage_MediumUrl",
                schema: "domain",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductImage_ThumbnailUrl",
                schema: "domain",
                table: "Products");

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "ItemModifiers", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(182), "{\"Size\":\"Small\",\"ExtraToppings\":[{\"ToppingId\":1,\"Quantity\":1},{\"ToppingId\":2,\"Quantity\":1}]}", new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(183) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(1789), new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(1790) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(1791), new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(1791) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(1794), new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(1795) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(1796), new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(1796) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(8778), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(8779) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(8784), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(8784) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(3308), new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(3112), new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(3308) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(3776), new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(3776) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(5082), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(5083) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7158), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7158) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7170), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7170) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7189), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7189) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7194), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7195) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7200), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7200) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7265), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7265) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7271), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7271) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7277), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7277) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7282), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7282) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7296), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7296) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7298), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7298) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7299), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(7299) });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "afe4bbba-0acd-47b5-a87c-bc17e46e7a04");

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "19618ac9-8895-43b6-adc0-20403262448e");

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3079), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3292) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3487), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3487) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3488), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3489) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3490), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3490) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3491), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3492) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3493), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3493) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3494), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3494) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3510), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3510) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3511), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3511) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3512), new DateTime(2025, 11, 20, 9, 43, 24, 858, DateTimeKind.Utc).AddTicks(3513) });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "41977e76-f5bc-4a75-a5f1-5a059920cdd0");

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "54792786-684f-4d36-b605-e88912f6eb34");
        }
    }
}
