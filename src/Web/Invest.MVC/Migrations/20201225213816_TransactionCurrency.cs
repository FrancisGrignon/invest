using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Invest.MVC.Migrations
{
    public partial class TransactionCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockHistory_Stocks_StockId",
                table: "StockHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Forexes_ForexId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ForexId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ForexId",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Transactions",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "StockId",
                table: "StockHistory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 12, 25, 21, 38, 16, 577, DateTimeKind.Utc).AddTicks(289));

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 12, 25, 21, 38, 16, 577, DateTimeKind.Utc).AddTicks(576));

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 3,
                column: "Created",
                value: new DateTime(2020, 12, 25, 21, 38, 16, 577, DateTimeKind.Utc).AddTicks(578));

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 4,
                column: "Created",
                value: new DateTime(2020, 12, 25, 21, 38, 16, 577, DateTimeKind.Utc).AddTicks(574));

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 5,
                column: "Created",
                value: new DateTime(2020, 12, 25, 21, 38, 16, 577, DateTimeKind.Utc).AddTicks(572));

            migrationBuilder.AddForeignKey(
                name: "FK_StockHistory_Stocks_StockId",
                table: "StockHistory",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockHistory_Stocks_StockId",
                table: "StockHistory");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "ForexId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "StockId",
                table: "StockHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 12, 25, 18, 47, 50, 180, DateTimeKind.Utc).AddTicks(8526));

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 12, 25, 18, 47, 50, 180, DateTimeKind.Utc).AddTicks(8849));

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 3,
                column: "Created",
                value: new DateTime(2020, 12, 25, 18, 47, 50, 180, DateTimeKind.Utc).AddTicks(8851));

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 4,
                column: "Created",
                value: new DateTime(2020, 12, 25, 18, 47, 50, 180, DateTimeKind.Utc).AddTicks(8847));

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 5,
                column: "Created",
                value: new DateTime(2020, 12, 25, 18, 47, 50, 180, DateTimeKind.Utc).AddTicks(8844));

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ForexId",
                table: "Transactions",
                column: "ForexId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockHistory_Stocks_StockId",
                table: "StockHistory",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "StockId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Forexes_ForexId",
                table: "Transactions",
                column: "ForexId",
                principalTable: "Forexes",
                principalColumn: "ForexId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
