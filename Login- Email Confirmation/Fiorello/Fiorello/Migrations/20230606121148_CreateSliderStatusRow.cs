using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiorello.Migrations
{
    public partial class CreateSliderStatusRow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "sliders",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "sliders");

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 3, 23, 33, 1, 784, DateTimeKind.Local).AddTicks(2534));

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 3, 23, 33, 1, 784, DateTimeKind.Local).AddTicks(2554));

            migrationBuilder.UpdateData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2023, 6, 3, 23, 33, 1, 784, DateTimeKind.Local).AddTicks(2555));
        }
    }
}
