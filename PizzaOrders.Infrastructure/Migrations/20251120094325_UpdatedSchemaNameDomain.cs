using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaOrders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSchemaNameDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "domain");

            migrationBuilder.RenameTable(
                name: "Toppings",
                newName: "Toppings",
                newSchema: "domain");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products",
                newSchema: "domain");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payments",
                newSchema: "domain");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Orders",
                newSchema: "domain");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                newName: "OrderItems",
                newSchema: "domain");

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(182), new DateTime(2025, 11, 20, 9, 43, 24, 859, DateTimeKind.Utc).AddTicks(183) });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Toppings",
                schema: "domain",
                newName: "Toppings");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "domain",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Payments",
                schema: "domain",
                newName: "Payments");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "domain",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "OrderItems",
                schema: "domain",
                newName: "OrderItems");

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(6114), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(6114) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(7697), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(7697) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(7699), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(7699) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(7702), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(7702) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(7703), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(7704) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(4783), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(4784) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(4789), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(4789) });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(9323), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(9125), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(9322) });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(9787), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(9787) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(51), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(51) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3055), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3056) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3131), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3132) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3151), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3152) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3157), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3158) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3163), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3163) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3175), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3176) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3181), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3181) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3186), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3187) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3192), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3192) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3204), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3204) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3205), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3206) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3207), new DateTime(2025, 11, 20, 9, 41, 26, 128, DateTimeKind.Utc).AddTicks(3207) });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "edc819a2-c684-468b-9aaa-453ad0f068ec");

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "447a9461-7c3f-4f6a-b183-262044e6f318");

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(7996), new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8213) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8412), new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8413) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8414), new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8414) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8416), new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8416) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8417), new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8417) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8418), new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8419) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8420), new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8420) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8435), new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8436) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8437), new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8437) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8438), new DateTime(2025, 11, 20, 9, 41, 26, 127, DateTimeKind.Utc).AddTicks(8438) });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "45d762f7-61a4-4104-811c-19bea23ccaee");

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "72121692-acca-4205-93b9-15e2116bc2e2");
        }
    }
}
