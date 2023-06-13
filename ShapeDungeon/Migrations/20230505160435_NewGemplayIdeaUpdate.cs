using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeDungeon.Migrations
{
    public partial class NewGemplayIdeaUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enemies_Rooms_RoomId",
                table: "Enemies");

            migrationBuilder.DropIndex(
                name: "IX_Enemies_RoomId",
                table: "Enemies");

            migrationBuilder.DropColumn(
                name: "NextRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PreviousRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Enemies");

            migrationBuilder.AddColumn<Guid>(
                name: "DownRoomId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EnemyId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LeftRoomId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RightRoomId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TopRoomId",
                table: "Rooms",
                type: "uniqueidentifier",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Enemies_EnemyId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_EnemyId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "DownRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "EnemyId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "LeftRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RightRoomId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "TopRoomId",
                table: "Rooms");

            migrationBuilder.AddColumn<Guid>(
                name: "NextRoomId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PreviousRoomId",
                table: "Rooms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "Enemies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Enemies_RoomId",
                table: "Enemies",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enemies_Rooms_RoomId",
                table: "Enemies",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
