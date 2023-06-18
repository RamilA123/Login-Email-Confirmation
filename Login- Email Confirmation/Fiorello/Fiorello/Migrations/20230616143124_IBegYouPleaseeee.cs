using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiorello.Migrations
{
    public partial class IBegYouPleaseeee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "Id", "Age", "CreatedDate", "FullName", "SoftDelete" },
                values: new object[,]
                {
                    { 1, 19, new DateTime(2023, 6, 16, 18, 31, 24, 301, DateTimeKind.Local).AddTicks(2415), "Musa Afandiyev", false },
                    { 2, 19, new DateTime(2023, 6, 16, 18, 31, 24, 301, DateTimeKind.Local).AddTicks(2430), "Murad Jafarov", false },
                    { 3, 6, new DateTime(2023, 6, 16, 18, 31, 24, 301, DateTimeKind.Local).AddTicks(2431), "Resul Hasanov", false }
                });

            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "Key", "Value" },
                values: new object[,]
                {
                    { 1, "Logo", "logo.png" },
                    { 2, "Phone", "+994504198914" },
                    { 3, "Email", "fiorello@code.edu.az" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "customers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
