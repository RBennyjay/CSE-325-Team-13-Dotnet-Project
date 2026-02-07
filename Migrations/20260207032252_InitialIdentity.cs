using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBudget.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dev-user-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6fe0efed-3356-42b6-9634-774e758a0502", new DateTime(2026, 2, 7, 3, 22, 51, 465, DateTimeKind.Utc).AddTicks(2560), "AQAAAAIAAYagAAAAEJ/2NuKd/e7Bc3U2W5S68paMk3vtw6R9EG41t6wAuKyKpLTrMRJ6C8HJQ+tDf1sTFg==", "19df09ca-b391-4ce4-84ac-2c2e335e952d" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 3, 22, 51, 467, DateTimeKind.Utc).AddTicks(2120));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 3, 22, 51, 467, DateTimeKind.Utc).AddTicks(3300));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 3, 22, 51, 467, DateTimeKind.Utc).AddTicks(3300));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 3, 22, 51, 467, DateTimeKind.Utc).AddTicks(3310));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 3, 22, 51, 467, DateTimeKind.Utc).AddTicks(3310));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 3, 22, 51, 467, DateTimeKind.Utc).AddTicks(3310));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dev-user-123",
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "PasswordHash", "SecurityStamp" },
                values: new object[] { "b1e87213-5d3b-4510-a3bf-234bc4a28c64", new DateTime(2026, 2, 7, 1, 49, 54, 561, DateTimeKind.Utc).AddTicks(2671), "AQAAAAIAAYagAAAAENhC24p1bwi+JCjs6HtuLNYJNbFMeOBBitvjdZzwW/LuwN/8PyHogY2Dies0Ui3gXA==", "dcec0ed3-c80e-4851-a192-0b3841fc88a9" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 1, 49, 54, 563, DateTimeKind.Utc).AddTicks(7615));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 1, 49, 54, 563, DateTimeKind.Utc).AddTicks(8626));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 1, 49, 54, 563, DateTimeKind.Utc).AddTicks(8634));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 1, 49, 54, 563, DateTimeKind.Utc).AddTicks(8640));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 1, 49, 54, 563, DateTimeKind.Utc).AddTicks(8646));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 7, 1, 49, 54, 563, DateTimeKind.Utc).AddTicks(8649));
        }
    }
}
