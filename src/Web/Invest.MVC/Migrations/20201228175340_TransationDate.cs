using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Invest.MVC.Migrations
{
    public partial class TransationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateUtc",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1282), new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1297) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1755), new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1756) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1758), new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1759) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1752), new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1753) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1748), new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1750) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1760), new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1761) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1763), new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1764) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1766), new DateTime(2020, 12, 28, 17, 53, 40, 260, DateTimeKind.Utc).AddTicks(1767) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUtc",
                table: "Transactions");

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(1548), new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(1565) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2282), new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2283) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2285), new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2286) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2279), new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2280) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2271), new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2276) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2288), new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2289) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2291), new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2292) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2294), new DateTime(2020, 12, 28, 16, 39, 44, 283, DateTimeKind.Utc).AddTicks(2296) });
        }
    }
}
