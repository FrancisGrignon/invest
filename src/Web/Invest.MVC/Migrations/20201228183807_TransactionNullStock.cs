using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Invest.MVC.Migrations
{
    public partial class TransactionNullStock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Stocks_StockId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "StockId",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Stocks_StockId",
                table: "Transactions",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Stocks_StockId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "StockId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Stocks_StockId",
                table: "Transactions",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
