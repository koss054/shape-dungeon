using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShapeDungeon.Migrations
{
    public partial class UpdatedPlayerAndRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanGoDown",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanGoLeft",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanGoRight",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanGoUp",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Shape",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanGoDown",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "CanGoLeft",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "CanGoRight",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "CanGoUp",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Shape",
                table: "Players");
        }
    }
}
