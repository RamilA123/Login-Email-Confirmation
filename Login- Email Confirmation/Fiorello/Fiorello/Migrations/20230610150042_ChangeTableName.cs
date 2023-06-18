using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiorello.Migrations
{
    public partial class ChangeTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 10, 19, 0, 42, 207, DateTimeKind.Local).AddTicks(2985));

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 10, 19, 0, 42, 207, DateTimeKind.Local).AddTicks(3000));

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 10, 19, 0, 42, 207, DateTimeKind.Local).AddTicks(3001));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 6, 16, 11, 48, 639, DateTimeKind.Local).AddTicks(2093));

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 6, 16, 11, 48, 639, DateTimeKind.Local).AddTicks(2105));

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 6, 16, 11, 48, 639, DateTimeKind.Local).AddTicks(2106));
        }
    }
}
