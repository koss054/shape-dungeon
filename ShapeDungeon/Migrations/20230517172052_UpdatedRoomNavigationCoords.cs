using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeDungeon.Migrations
{
    public partial class UpdatedRoomNavigationCoords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownRoomId",
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

            migrationBuilder.AddColumn<int>(
                name: "CoordX",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoordY",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoordX",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "CoordY",
                table: "Rooms");

            migrationBuilder.AddColumn<Guid>(
                name: "DownRoomId",
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
        }
    }
}
