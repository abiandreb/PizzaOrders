using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaOrders.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserSeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(2026), new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(2027) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(4268), new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(4269) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(4271), new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(4272) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(4276), new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(4277) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(4278), new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(4279) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(359), new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(361) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Orders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(366), new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(367) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Payments",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConfirmedAt", "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(6469), new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(6225), new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(6469) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Payments",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(7234), new DateTime(2026, 1, 15, 14, 52, 30, 669, DateTimeKind.Utc).AddTicks(7235) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(4001), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(4002) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8187), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8188) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8205), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8206) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8213), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8213) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8236), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8236) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8243), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8244) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8250), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8250) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8267), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8268) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8275), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8276) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8372), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8372) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8383), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8384) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8404), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8404) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Products",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8407), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(8407) });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "a9d63d15-634a-4648-9194-79317f9bcb8f");

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "25674256-98bd-4788-82ec-6bef2aa871d3");

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(751), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1016) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1256), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1256) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1258), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1259) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1260), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1260) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1262), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1262) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1263), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1264) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1265), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1266) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1267), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1267) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1269), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1269) });

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "Toppings",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1270), new DateTime(2026, 1, 15, 14, 52, 30, 668, DateTimeKind.Utc).AddTicks(1271) });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "e98196a9-83c2-485d-8ab4-ca44e29b0ff5", "admin@pizzaorders.com", "ADMIN@PIZZAORDERS.COM", "ADMIN@PIZZAORDERS.COM", "AQAAAAEAACcQAAAAECM8urhnYbjZrzD1hm7LpGKIzTlpRCtRn3xjyDrluo3SrVMaSio9XINiO2ZHm9kV6Q==", "41827729-f834-4037-ac06-95391634b9c8", "admin@pizzaorders.com" });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "935d246d-2102-4dc5-b960-2e9bfd83a11f", "user@pizzaorders.com", "USER@PIZZAORDERS.COM", "USER@PIZZAORDERS.COM", "AQAAAAEAACcQAAAAEIZDa3uUwJeh7hemK0lcE9Q7dgcZnGXUPuTAs1wvSIWwaAi4v6AW82fFkRlWHwzrGQ==", "d000d945-dd1f-44a6-9a18-e693e3da43c5", "user@pizzaorders.com" });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "Users",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsGuest", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { 3, 0, null, "9122f41c-cbd7-48d3-8b69-7de1cc6b71d3", "test@pizzaorders.com", true, false, false, null, "TEST@PIZZAORDERS.COM", "TEST@PIZZAORDERS.COM", "AQAAAAEAACcQAAAAEPGiCJ2u8hbbqfojZeJjZJBKSBtai0edJrx1y+zegh6wN6+YSWt/Q6F2Jjn22KkZgQ==", null, false, "4de880d7-6c2e-47fd-98c2-4e1a5253055b", false, "test@pizzaorders.com" });

            migrationBuilder.InsertData(
                schema: "identity",
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId", "Id" },
                values: new object[] { 2, 3, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "identity",
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                schema: "domain",
                table: "OrderItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(1983), new DateTime(2026, 1, 15, 14, 30, 28, 229, DateTimeKind.Utc).AddTicks(1984) });

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
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "4468d8bd-8b88-4d2e-8555-18e54131bae6", "admin@example.com", "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEJ/n/y4nL5z0mJ6/BEiXzY3T2gR8gUaZa/tG2gR8gUaZa/tG2gR8gUaZa/tG2g==", "9239922b-2875-4350-a926-2184462719a4", "admin" });

            migrationBuilder.UpdateData(
                schema: "identity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "02df7502-4e63-4185-a9f2-482474f3809d", "user@example.com", "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAEJ/n/y4nL5z0mJ6/BEiXzY3T2gR8gUaZa/tG2gR8gUaZa/tG2gR8gUaZa/tG2g==", "e178d197-36e7-4935-a744-83a3162b77a7", "user" });
        }
    }
}
