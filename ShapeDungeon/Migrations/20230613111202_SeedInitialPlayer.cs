using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeDungeon.Migrations
{
    public partial class SeedInitialPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "Agility", "CurrentExp", "CurrentScoutEnergy", "CurrentSkillpoints", "ExpToNextLevel", "IsActive", "Level", "Name", "Shape", "Strength", "Vigor" },
                values: new object[] { new Guid("3de35703-1fef-4070-d75d-08db4beac0a7"), 1, 0, 1, 0, 100, true, 0, "Squary Lvl.8", 0, 2, 5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: new Guid("3de35703-1fef-4070-d75d-08db4beac0a7"));
        }
    }
}
