using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Invest.MVC.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forexes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    ExchangeRate = table.Column<float>(type: "REAL", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forexes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Investors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Value = table.Column<float>(type: "REAL", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForexHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ForexId = table.Column<int>(type: "INTEGER", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    ExchangeRate = table.Column<float>(type: "REAL", nullable: false),
                    DateUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForexHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForexHistories_Forexes_ForexId",
                        column: x => x.ForexId,
                        principalTable: "Forexes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Investments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StockId = table.Column<int>(type: "INTEGER", nullable: false),
                    InvestorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<float>(type: "REAL", precision: 18, scale: 5, nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Investments_Investors_InvestorId",
                        column: x => x.InvestorId,
                        principalTable: "Investors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Investments_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StockId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Symbol = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Value = table.Column<float>(type: "REAL", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    DateUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockHistories_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StockId = table.Column<int>(type: "INTEGER", nullable: true),
                    OperationId = table.Column<int>(type: "INTEGER", nullable: false),
                    InvestorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<float>(type: "REAL", nullable: false),
                    Amount = table.Column<float>(type: "REAL", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    ExchangeRate = table.Column<float>(type: "REAL", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    DateUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Investors_InvestorId",
                        column: x => x.InvestorId,
                        principalTable: "Investors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvestmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    StockId = table.Column<int>(type: "INTEGER", nullable: false),
                    InvestorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<float>(type: "REAL", precision: 18, scale: 5, nullable: false),
                    Value = table.Column<float>(type: "REAL", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    ExchangeRate = table.Column<float>(type: "REAL", precision: 18, scale: 5, nullable: false),
                    DateUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Enable = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentHistories_Investments_InvestmentId",
                        column: x => x.InvestmentId,
                        principalTable: "Investments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InvestmentHistories_Investors_InvestorId",
                        column: x => x.InvestorId,
                        principalTable: "Investors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestmentHistories_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "CreatedUtc", "Enable", "Name", "UpdatedUtc" },
                values: new object[] { 1, new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(655), true, "Buy", new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(665) });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "CreatedUtc", "Enable", "Name", "UpdatedUtc" },
                values: new object[] { 5, new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1078), true, "Dividend", new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1081) });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "CreatedUtc", "Enable", "Name", "UpdatedUtc" },
                values: new object[] { 4, new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1083), true, "Merge", new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1084) });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "CreatedUtc", "Enable", "Name", "UpdatedUtc" },
                values: new object[] { 2, new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1086), true, "Sell", new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1087) });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "CreatedUtc", "Enable", "Name", "UpdatedUtc" },
                values: new object[] { 3, new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1089), true, "Split", new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1090) });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "CreatedUtc", "Enable", "Name", "UpdatedUtc" },
                values: new object[] { 6, new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1092), true, "Deposit", new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1093) });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "CreatedUtc", "Enable", "Name", "UpdatedUtc" },
                values: new object[] { 7, new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1095), true, "Withdraw", new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1096) });

            migrationBuilder.InsertData(
                table: "Operations",
                columns: new[] { "Id", "CreatedUtc", "Enable", "Name", "UpdatedUtc" },
                values: new object[] { 8, new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1098), true, "Transfer", new DateTime(2021, 11, 14, 18, 35, 51, 969, DateTimeKind.Utc).AddTicks(1099) });

            migrationBuilder.CreateIndex(
                name: "IX_Forexes_Currency",
                table: "Forexes",
                column: "Currency");

            migrationBuilder.CreateIndex(
                name: "IX_ForexHistories_ForexId",
                table: "ForexHistories",
                column: "ForexId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentHistories_InvestmentId",
                table: "InvestmentHistories",
                column: "InvestmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentHistories_InvestorId",
                table: "InvestmentHistories",
                column: "InvestorId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentHistories_StockId",
                table: "InvestmentHistories",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_InvestorId",
                table: "Investments",
                column: "InvestorId");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_StockId",
                table: "Investments",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockHistories_StockId",
                table: "StockHistories",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockHistories_Symbol",
                table: "StockHistories",
                column: "Symbol");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_Symbol",
                table: "Stocks",
                column: "Symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_InvestorId",
                table: "Transactions",
                column: "InvestorId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OperationId",
                table: "Transactions",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_StockId",
                table: "Transactions",
                column: "StockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForexHistories");

            migrationBuilder.DropTable(
                name: "InvestmentHistories");

            migrationBuilder.DropTable(
                name: "StockHistories");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Forexes");

            migrationBuilder.DropTable(
                name: "Investments");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "Investors");

            migrationBuilder.DropTable(
                name: "Stocks");
        }
    }
}
