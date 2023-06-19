using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeDungeon.Migrations
{
    public partial class RemoveEnemyPropFromRoomEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Enemies_EnemyId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_EnemyId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "EnemyId",
                table: "Rooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EnemyId",
                table: "Rooms",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_EnemyId",
                table: "Rooms",
                column: "EnemyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Enemies_EnemyId",
                table: "Rooms",
                column: "EnemyId",
                principalTable: "Enemies",
                principalColumn: "Id");
        }
    }
}
