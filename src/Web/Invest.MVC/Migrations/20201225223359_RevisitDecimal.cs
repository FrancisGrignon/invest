using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Invest.MVC.Migrations
{
    public partial class RevisitDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 1,
                column: "Created",
                value: new DateTime(2020, 12, 25, 22, 33, 58, 876, DateTimeKind.Utc).AddTicks(8885));

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 2,
                column: "Created",
                value: new DateTime(2020, 12, 25, 22, 33, 58, 876, DateTimeKind.Utc).AddTicks(9373));

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 3,
                column: "Created",
                value: new DateTime(2020, 12, 25, 22, 33, 58, 876, DateTimeKind.Utc).AddTicks(9374));

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 4,
                column: "Created",
                value: new DateTime(2020, 12, 25, 22, 33, 58, 876, DateTimeKind.Utc).AddTicks(9371));

            migrationBuilder.UpdateData(
                table: "Operations",
                keyColumn: "OperationId",
                keyValue: 5,
                column: "Created",
                value: new DateTime(2020, 12, 25, 22, 33, 58, 876, DateTimeKind.Utc).AddTicks(9368));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
