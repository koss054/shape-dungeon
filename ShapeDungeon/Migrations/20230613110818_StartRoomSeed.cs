using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeDungeon.Migrations
{
    public partial class StartRoomSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "CanGoDown", "CanGoLeft", "CanGoRight", "CanGoUp", "CoordX", "CoordY", "EnemyId", "IsActiveForEdit", "IsActiveForMove", "IsActiveForScout", "IsEndRoom", "IsEnemyRoom", "IsSafeRoom", "IsStartRoom" },
                values: new object[] { new Guid("dd54f8ee-349f-4dfd-1a70-08db56fb8a4b"), true, true, true, true, 0, 0, null, true, true, true, false, false, false, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Rooms",
                keyColumn: "Id",
                keyValue: new Guid("dd54f8ee-349f-4dfd-1a70-08db56fb8a4b"));
        }
    }
}
