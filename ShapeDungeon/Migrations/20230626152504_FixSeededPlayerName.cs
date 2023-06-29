using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeDungeon.Migrations
{
    public partial class FixSeededPlayerName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("3de35703-1fef-4070-d75d-08db4beac0a7"),
                column: "Name",
                value: "Squary");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("3de35703-1fef-4070-d75d-08db4beac0a7"),
                column: "Name",
                value: "Squary Lvl.8");
        }
    }
}
