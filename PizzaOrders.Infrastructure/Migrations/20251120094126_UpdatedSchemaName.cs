using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaOrders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSchemaName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                newName: "UserTokens",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoles",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                newName: "UserLogins",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newName: "UserClaims",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                newName: "RoleClaims",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "RefreshTokens",
                newSchema: "identity");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UserTokens",
                schema: "identity",
                newName: "UserTokens");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "identity",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "identity",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                schema: "identity",
                newName: "UserLogins");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                schema: "identity",
                newName: "UserClaims");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "identity",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                schema: "identity",
                newName: "RoleClaims");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                schema: "identity",
                newName: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(6718), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(6718) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8261), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8262) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8263), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8264) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8267), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8268) });

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8269), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(8269) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(5395), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(5396) });

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(5400), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(5400) });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 216, DateTimeKind.Utc).AddTicks(106), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(9887), new DateTime(2025, 11, 17, 16, 44, 14, 216, DateTimeKind.Utc).AddTicks(105) });

            migrationBuilder.UpdateData(
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 216, DateTimeKind.Utc).AddTicks(585), new DateTime(2025, 11, 17, 16, 44, 14, 216, DateTimeKind.Utc).AddTicks(586) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(1814), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(1815) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3776), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3776) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3788), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3788) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3794), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3794) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3866), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3867) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3873), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3873) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3878), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3879) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3894), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3894) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3900), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3900) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3905), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3905) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3918), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3918) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3920), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3920) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3921), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(3922) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5a117e0a-f074-4954-a915-e81ede6bfaa9");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "1e5f5fa0-cf31-4861-bcfc-118962586af9");

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 214, DateTimeKind.Utc).AddTicks(9780), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(11) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(207), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(208) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(209), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(210) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(211), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(212) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(213), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(213) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(214), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(214) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(215), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(216) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(217), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(217) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(218), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(218) });

            migrationBuilder.UpdateData(
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(219), new DateTime(2025, 11, 17, 16, 44, 14, 215, DateTimeKind.Utc).AddTicks(220) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "ef4576ca-01dd-42a9-a8dd-2200852bcba0");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "a267eb5d-5e8e-408a-8c29-f8d0d6cbc080");
        }
    }
}
