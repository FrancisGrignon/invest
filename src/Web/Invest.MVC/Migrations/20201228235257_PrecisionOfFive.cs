using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Invest.MVC.Migrations
{
    public partial class PrecisionOfFive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Investments",
                type: "decimal(18,5)",
                precision: 18,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "InvestmentHistories",
                type: "decimal(18,5)",
                precision: 18,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRate",
                table: "InvestmentHistories",
                type: "decimal(18,5)",
                precision: 18,
                scale: 5,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6405), new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6416) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6852), new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6853) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6854), new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6856) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6849), new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6850) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6845), new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6847) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6857), new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6858) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6860), new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6861) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6862), new DateTime(2020, 12, 28, 23, 52, 56, 915, DateTimeKind.Utc).AddTicks(6864) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "Investments",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,5)",
                oldPrecision: 18,
                oldScale: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "InvestmentHistories",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,5)",
                oldPrecision: 18,
                oldScale: 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRate",
                table: "InvestmentHistories",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,5)",
                oldPrecision: 18,
                oldScale: 5);

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(2834), new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(2850) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3427), new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3429) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3431), new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3432) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3424), new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3425) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3419), new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3422) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3434), new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3436) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3437), new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3439) });

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedUtc", "UpdatedUtc" },
                values: new object[] { new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3441), new DateTime(2020, 12, 28, 18, 38, 7, 456, DateTimeKind.Utc).AddTicks(3442) });
        }
    }
}
